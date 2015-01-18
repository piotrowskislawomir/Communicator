using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Communicator.BusinessLayer.Interfaces;
using Communicator.BusinessLayer.Models;
using Communicator.Protocol.Enums;
using Communicator.Protocol.Model;
using Communicator.Protocol.Notifications;
using Communicator.Protocol.Requests;
using Communicator.Protocol.Responses;
using Communicator.Queue;
using Communicator.Queue.Interfaces;
using Communicator.Untils.Archivizers.Message;
using Communicator.Untils.Interfaces;
using RabbitMQ.Client.Events;

namespace Communicator.BusinessLayer.Services
{
    public class MessageRecognizerService : IMessageRecognizerService
    {
        private readonly IQueueManagerService _queueManagerService;
        private readonly ISerializerService _serializerService;
        private readonly ICommonUserListService _commonUserListService;
        private readonly IMessageArchivizer _messageArchivizer;
        public IConfigurationService ConfigurationService { get; set; }
        public IQueueServerService QueueServerService { get; set; }

        private readonly IDictionary<User, UserDetails> _currentUsers = new Dictionary<User, UserDetails>();

        public MessageRecognizerService(IQueueManagerService queueManagerService, ISerializerService serializerService, ICommonUserListService commonUserListService, IMessageArchivizer messageArchivizer)
        {
            _queueManagerService = queueManagerService;
            _serializerService = serializerService;
            _commonUserListService = commonUserListService;
            _messageArchivizer = messageArchivizer;
        }

        public void Initialize()
        {
            _queueManagerService.Initialize(ConfigurationService.Host, ConfigurationService.UserName, ConfigurationService.Password, ConfigurationService.ExchangeName);
            _commonUserListService.FilePath = ConfigurationService.UserListFileName;
            _commonUserListService.LoadAllUsersFromFile();

            InitTimer();
        }

        private void InitTimer()
        {
            Task.Factory.StartNew(Timer);
        }

        private void Timer()
        {
            while (true)
            {
                try
                {

                
                var userToDelete = _currentUsers.Where(cu => cu.Value.ActivityTime < DateTime.Now.AddSeconds(-40));
                foreach (var userDel in userToDelete)
                {
                    _currentUsers.Remove(userDel);

                    var presenceStatusNotification = new PresenceStatusNotification
                    {
                        Login = userDel.Key.Login,
                        PresenceStatus = PresenceStatus.Offline
                    };

                    foreach (var user in _currentUsers.Keys)
                    {
                        _currentUsers[user].TopicList.ToList().ForEach(topic =>
                        {
                            QueueServerService.SendData(topic, ConfigurationService.ExchangeName,
                                presenceStatusNotification);
                        });
                    }
                }
                }
                catch (Exception)
                {
                }

                Thread.Sleep(30000);
            }
        }

        public void ProcessMessage(MessageReceivedEventArgs message)
        {
            try
            {
                var type = Type.GetType(message.ContentType);

                if (type == typeof(CreateUserReq))
                {
                    CreateUserProcess(message);
                    return;
                }

                if (type == typeof(AuthRequest))
                {
                    AuthUserProcess(message);
                    return;
                }

                if (type == typeof(MessageReq))
                {
                    MessageProcess(message);
                    return;
                }

                if (type == typeof(UserListReq))
                {
                    UserListProcess(message);
                    return;
                }

                if (type == typeof(ActivityReq))
                {
                    ActivityProcess(message);
                    return;
                }

                if (type == typeof(PresenceStatusNotification)) 
                {
                    PresenceStatusNotificationProcess(message);
                    return;
                }

                if (type == typeof (HistoryReq))
                {
                    HistoryProcess(message);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void HistoryProcess(MessageReceivedEventArgs message)
        {
            var historyReq = _serializerService.Deserialize<HistoryReq>(message.Message);

            List<MessageNotification> historyMessages = _messageArchivizer.Read(historyReq.Login,
                ConfigurationService.ArchiveFileName);


            var historyResponse = new HistoryResponse
            {
                Messages = historyMessages
            };

            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, historyResponse);
        }

        private void PresenceStatusNotificationProcess(MessageReceivedEventArgs message)
        {
            var presenceStatusNotification = _serializerService.Deserialize<PresenceStatusNotification>(message.Message);

            var activeUser = _currentUsers.SingleOrDefault(u => u.Key.Login == presenceStatusNotification.Login);

            if (activeUser.Key != null)
            {
                activeUser.Value.ActivityTime = DateTime.Now;
                if (activeUser.Key.Status != presenceStatusNotification.PresenceStatus)
                {
                    activeUser.Key.Status = presenceStatusNotification.PresenceStatus;

                    foreach (var user in _currentUsers.Keys)
                    {
                        _currentUsers[user].TopicList.ToList().ForEach(topic =>
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

            var activeUser = _currentUsers.SingleOrDefault(u => u.Key.Login == activityRequest.Recipient);

            if (activeUser.Key != null)
            {
                activeUser.Value.TopicList.ToList().ForEach(topic =>
                {
                    QueueServerService.SendData(topic, ConfigurationService.ExchangeName, activityNotification);
                });
            }

            QueueServerService.SendData(activityRequest.Recipient, ConfigurationService.ExchangeName, activityNotification);

            var activityResponse = new ActivityResponse();
            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, activityResponse);
        }

        private void UserListProcess(MessageReceivedEventArgs message)
        {
            var userListRequest = _serializerService.Deserialize<UserListReq>(message.Message);
            var allUsers = _commonUserListService.GetUsers();
            var userListResponse = new UserListResponse();
            userListResponse.Users = new List<User>();

            foreach (var user in _currentUsers.Keys.Where(u => u.Login != userListRequest.Login))
            {
                userListResponse.Users.Add(user);
            }

            foreach (var user in allUsers)
            {
                if (user.Login != userListRequest.Login)
                {
                    if (!userListResponse.Users.Any(u => u.Login == user.Login))
                    {
                        userListResponse.Users.Add(new User() { Login = user.Login, Status = PresenceStatus.Offline });
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
                    topic.Value.TopicList.ToList().ForEach(t =>
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
            msgRequest.SendTime = DateTimeOffset.Now;
            bool userInstanceExists = false;

            bool avaliable = _commonUserListService.UserExist(msgRequest);

            var activeUser = _currentUsers.SingleOrDefault(u => u.Key.Login == msgRequest.Recipient);
            if (activeUser.Key != null)
            {
                activeUser.Value.TopicList.ToList().ForEach(topic =>
                {
                    QueueServerService.SendData(topic,
                        ConfigurationService.ExchangeName,
                        msgRequest);
                    userInstanceExists = true;
                });
            }

            if (avaliable && !userInstanceExists)
            {
                QueueServerService.SendData(String.Format("archive.{0}", msgRequest.Recipient),
                    ConfigurationService.ExchangeName,
                    msgRequest);
            }

            var messageResponse = new MessageResponse();
            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, messageResponse);

          
            _messageArchivizer.Save(msgRequest, ConfigurationService.ArchiveFileName);
        }

        private void AuthUserProcess(MessageReceivedEventArgs message)
        {
            var authRequest = _serializerService.Deserialize<AuthRequest>(message.Message);

            bool exists = _commonUserListService.UserAuthentication(authRequest);

            if (exists)
            {
                var activeUser = _currentUsers.SingleOrDefault(u => u.Key.Login == authRequest.Login);
                if (activeUser.Key == null)
                {
                    var topicList = new List<string> {message.TopicSender};
                    _currentUsers.Add(new User {Login = authRequest.Login, Status = PresenceStatus.Online},
                        new UserDetails {ActivityTime = DateTime.Now, TopicList = topicList});
                }
                else
                {

                    if (!activeUser.Value.TopicList.Contains(message.TopicSender))
                    {
                        activeUser.Value.TopicList.Add(message.TopicSender);
                    }
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
                    _currentUsers[user].TopicList.ToList().ForEach(topic =>
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

            bool success = _commonUserListService.CreateNewUser(createUserRequest);
            if (success)
            {
                _commonUserListService.LoadAllUsersFromFile();
            }
            var createUserResponse = new CreateUserResponse
            {
                CreatedSuccessfully = success
            };

            QueueServerService.SendData(message.TopicSender, ConfigurationService.ExchangeName, createUserResponse);
        }
    }
}
