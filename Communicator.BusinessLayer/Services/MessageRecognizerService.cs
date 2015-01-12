﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Communicator.BusinessLayer.Interfaces;
using Communicator.Protocol.Enums;
using Communicator.Protocol.Model;
using Communicator.Protocol.Notifications;
using Communicator.Protocol.Requests;
using Communicator.Protocol.Responses;
using Communicator.Queue;
using Communicator.Queue.Interfaces;
using Communicator.Untils.Configuration;
using Communicator.Untils.Serializers;
using Communicator.Untils.Archivizers.UsersList;
using RabbitMQ.Client.Events;

namespace Communicator.BusinessLayer.Services
{
    public class MessageRecognizerService: IMessageRecognizerService
    {
        private readonly IQueueManagerService _queueManagerService;
        private readonly ISerializerService _serializerService;
        private readonly ICommonUserListService _commonUserListService;
        public IConfigurationService ConfigurationService { get; set; }
        public IQueueServerService QueueServerService { get; set; }

        private readonly IDictionary<User, ICollection<string>> _currentUsers = new Dictionary<User, ICollection<string>>();

        public MessageRecognizerService(IQueueManagerService queueManagerService,  ISerializerService serializerService, ICommonUserListService commonUserListService)
        {
            _queueManagerService = queueManagerService;
            _serializerService = serializerService;
            _commonUserListService = commonUserListService;

        }

        public void Initialize()
        {
            _queueManagerService.Initialize(ConfigurationService.Host, ConfigurationService.UserName, ConfigurationService.Password, ConfigurationService.ExchangeName);
            _commonUserListService.FilePath = ConfigurationService.UserListFileName;
            _commonUserListService.LoadAllUsersFromFile();
        }

        public void ProcessMessage(MessageReceivedEventArgs message)
        {
            try
            {
                
                var type = Type.GetType(message.ContentType);

                if (type == typeof (CreateUserReq))
                {
                    CreateUserProcess(message);
                    return;
                }

                if (type == typeof (AuthRequest))
                {
                    AuthUserProcess(message);
                    return;
                }

                if (type == typeof (MessageReq))
                {
                    MessageProcess(message);
                    return;
                }

                if (type == typeof (UserListReq))
                {
                    UserListProcess(message);
                    return;
                }

                if (type == typeof (ActivityReq))
                {
                    ActivityProcess(message);
                    return;
                }

                if (type == typeof (ActivityNotification)) // PresestStatusNotification
                {
                    PresenceStatusNotificationProcess(message);
                    return;
                }
            }
            catch (Exception ex)
            {
                {
                }
                throw;
            }
        }

        private void PresenceStatusNotificationProcess(MessageReceivedEventArgs message)
        {
            var presenceStatusNotification = _serializerService.Deserialize<PresenceStatusNotification>(message.Message);

            //jesli bedzie roznil sie od obecnego to wysylaj do wszystkich
            foreach (var login in _currentUsers.Keys)
            {
                QueueServerService.SendData(string.Format("client.{0}", login), ConfigurationService.ExchangeName, presenceStatusNotification);
            }
        } 

        private void ActivityProcess(MessageReceivedEventArgs message)
        {
            var activityRequest = _serializerService.Deserialize<ActivityReq>(message.Message);

            var activityNotification = new ActivityNotification
            {
                Sender = activityRequest.Login,
                IsWriting = activityRequest.IsWriting
            };

            QueueServerService.SendData(activityRequest.Recipient, ConfigurationService.ExchangeName, activityNotification);
        
            var activityResponse = new ActivityResponse();
            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, activityResponse);
        }

        private void UserListProcess(MessageReceivedEventArgs message)
        {
            var userListRequest = _serializerService.Deserialize<UserListReq>(message.Message);

            //DONE//TODO lista wszystkich uzytkownikow
            //var userListResponse = new UserListResponse {Users = new List<User>()};
            // zwraca listę wszystkich aktywnych użytkowników z wyłączeniem użytkownika który ją wywołuje
            var allUsers = ActivityUserList.GetList();
            var userListResponse = new UserListResponse();
            userListResponse.Users = new List<User>();

            foreach (var user in _currentUsers.Keys.Where(u => u .Login != userListRequest.Login))
            {
                userListResponse.Users.Add(user);
            }

            foreach (var user in allUsers)
            {
                if (userListResponse.Users.All(u => u.Login != user.Login))
                {
                    if (user.Login != userListRequest.Login)
                    {
                        userListResponse.Users.Add(user);
                    }
                }
            }

            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, userListResponse);
        }

        private void MessageProcess(MessageReceivedEventArgs message)
        {
            var msgRequest = _serializerService.Deserialize<MessageReq>(message.Message);
            bool userInstanceExists = false;

            //TODO spr czy ta osoba jest zarejestrowana
            bool avaliable = _commonUserListService.UserExist(msgRequest);

            var activeUser = _currentUsers.SingleOrDefault(u => u.Key.Login == msgRequest.Login);
            if (activeUser.Key != null)
            {
                if (activeUser.Value.Any())
                {
                    QueueServerService.SendData(String.Format("client.{0}", msgRequest.Recipient),
                        ConfigurationService.ExchangeName,
                        msgRequest.Message);
                    userInstanceExists = true;
                }
            }

            if (!userInstanceExists)
            {
                QueueServerService.SendData(String.Format("archive.{0}", msgRequest.Recipient),
                    ConfigurationService.ExchangeName,
                    msgRequest.Message);
            }

            var messageResponse = new MessageResponse();
            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, messageResponse);

            //archiwizacja
                        
        }

        private void AuthUserProcess(MessageReceivedEventArgs message)
        {
            var authRequest = _serializerService.Deserialize<AuthRequest>(message.Message);

            //DONE////TODO sprawdzanie czy istnieje taki login i pass
            bool exists = _commonUserListService.UserAuthentication(authRequest);

            var activeUser = _currentUsers.SingleOrDefault(u => u.Key.Login == authRequest.Login);
            if (activeUser.Key == null)
            {
                var topicList = new List<string> {message.TopicSender};
                _currentUsers.Add(new User{Login = authRequest.Login, Status = PresenceStatus.Online}, topicList);
            }
            else
            {

                if (!activeUser.Value.Contains(message.TopicSender))
                {
                    activeUser.Value.Add(message.TopicSender);
                }
            }

            var consumer = _queueManagerService.CreateConsumerForClient(string.Format("archive.{0}", authRequest.Login));
            while (true)
            {
                BasicDeliverEventArgs basicDeliverEventArgs;
                consumer.Queue.Dequeue(50, out basicDeliverEventArgs);

                if (basicDeliverEventArgs == null)
                {
                    break;
                }

                //to moze niepoprawnie dzialac
                QueueServerService.SendData(message.TopicSender,
                    ConfigurationService.ExchangeName,
                    basicDeliverEventArgs.Body);
            }

            var authResponse = new AuthResponse
            {
                IsAuthenticated = exists
            };

            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, authResponse);


            var presenceStatusNotification = new PresenceStatusNotification
            {
                Login = authRequest.Login,
                PresenceStatus = PresenceStatus.Online
            };

            foreach (var login in _currentUsers.Keys)
            {
                QueueServerService.SendData(String.Format("client.{0}", login.Login),
                    ConfigurationService.ExchangeName, presenceStatusNotification);
            }


        }

        private void CreateUserProcess(MessageReceivedEventArgs message)
        {
            var createUserRequest = _serializerService.Deserialize<CreateUserReq>(message.Message); // hasło i login
            _queueManagerService.CreateQueue(string.Format("archive.{0}", createUserRequest.Login), ConfigurationService.ExchangeName);

            //DONE//TODO sprawdzanie czy juz istnieje
            
            var createUserResponse = new CreateUserResponse
            {
                CreatedSuccessfully = _commonUserListService.CreateNewUser(createUserRequest)
               //CreatedSuccessfully = true
            };

            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, createUserResponse);
        }
    }
}
