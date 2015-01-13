using System;
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

                if (type == typeof (PresenceStatusNotification)) // PresestStatusNotification
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

            var activeUser = _currentUsers.SingleOrDefault(u => u.Key.Login == presenceStatusNotification.Login);

            if (activeUser.Key != null)
            {
                if (activeUser.Key.Status != presenceStatusNotification.PresenceStatus)
                {
                    activeUser.Key.Status = presenceStatusNotification.PresenceStatus;
                    foreach (var user in _currentUsers.Keys)
                    {
                        _currentUsers[user].ToList().ForEach(topic =>
                        {
                            QueueServerService.SendData(topic, ConfigurationService.ExchangeName, presenceStatusNotification);
                        });
                    }
                }
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
            var allUsers = _commonUserListService.GetUsers();
            var userListResponse = new UserListResponse();
            userListResponse.Users = new List<User>();

            foreach (var user in _currentUsers.Keys.Where(u => u .Login != userListRequest.Login))
            {
                userListResponse.Users.Add(user);
            }

            foreach (var user in allUsers)
            {
                if (user.Login != userListRequest.Login)
                {
                    if (!userListResponse.Users.Any(u => u.Login == user.Login))
                    {
                        userListResponse.Users.Add(new User(){Login = user.Login, Status = PresenceStatus.Offline});
                    }
                }
            }

            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, userListResponse);

             var consumer = _queueManagerService.CreateConsumerForClient(string.Format("archive.{0}", userListRequest.Login));
            while (true)
            {
                BasicDeliverEventArgs basicDeliverEventArgs;
                consumer.Queue.Dequeue(50, out basicDeliverEventArgs);

                if (basicDeliverEventArgs == null)
                {
                    break;
                }

                var messageReq = _serializerService.Deserialize<MessageReq>(basicDeliverEventArgs.Body);

                _currentUsers.Where(u => u.Key.Login == userListRequest.Login).ToList().ForEach(topic =>
                {
                    topic.Value.ToList().ForEach(t =>
                    {
                        QueueServerService.SendData(t, ConfigurationService.ExchangeName, messageReq);
                    });
                });
                _queueManagerService.SendAck(basicDeliverEventArgs.DeliveryTag);
            }
        }

        private void MessageProcess(MessageReceivedEventArgs message)
        {
            var msgRequest = _serializerService.Deserialize<MessageReq>(message.Message);
            bool userInstanceExists = false;

            //TODO spr czy ta osoba jest zarejestrowana
            bool avaliable = _commonUserListService.UserExist(msgRequest);

            var activeUser = _currentUsers.SingleOrDefault(u => u.Key.Login == msgRequest.Recipient);
            if (activeUser.Key != null)
            {
                activeUser.Value.ToList().ForEach(topic =>
                {
                    QueueServerService.SendData(topic,
                        ConfigurationService.ExchangeName,
                        msgRequest);
                    userInstanceExists = true;
                });
            }

            if (!userInstanceExists)
            {
                QueueServerService.SendData(String.Format("archive.{0}", msgRequest.Recipient),
                    ConfigurationService.ExchangeName,
                    msgRequest);
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

           
            var authResponse = new AuthResponse
            {
                IsAuthenticated = exists
            };

            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, authResponse);

            if (exists)
            {
                var presenceStatusNotification = new PresenceStatusNotification
                {
                    Login = authRequest.Login,
                    PresenceStatus = PresenceStatus.Online
                };

                foreach (var user in _currentUsers.Keys)
                {
                    _currentUsers[user].ToList().ForEach(topic =>
                    {
                        QueueServerService.SendData(topic, ConfigurationService.ExchangeName, presenceStatusNotification);
                    });

                }
            }
        }

        private void CreateUserProcess(MessageReceivedEventArgs message)
        {
            var createUserRequest = _serializerService.Deserialize<CreateUserReq>(message.Message); // hasło i login
            _queueManagerService.CreateQueue(string.Format("archive.{0}", createUserRequest.Login), ConfigurationService.ExchangeName);

            //DONE//TODO sprawdzanie czy juz istnieje

            bool success = _commonUserListService.CreateNewUser(createUserRequest);
            if (success)
            {
                _commonUserListService.LoadAllUsersFromFile();
            }
            var createUserResponse = new CreateUserResponse
            {
                CreatedSuccessfully = success
               //CreatedSuccessfully = true
            };

            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, createUserResponse);
        }
    }
}
