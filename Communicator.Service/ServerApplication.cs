using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Protocol.Requests;
using Communicator.Queue;
using Communicator.Queue.Interfaces;
using Communicator.Queue.Services;
using Communicator.Untils;
using Communicator.Untils.Serializers;
using RabbitMQ.Client.Events;

namespace Communicator.Server
{
    public class ServerApplication
    {
        private readonly IDictionary _currentUsers = new Dictionary<string, ICollection<string>>();
        public void Start()
        {
            Task.Factory.StartNew(CreateNewListener);
        }

        private void CreateNewListener()
        {
            

            var queueServerService = new RabbitMqServerService(new RabbitMqConnection());
            queueServerService.Initialize(ConfigurationApp.Host, ConfigurationApp.UserName, ConfigurationApp.Password, ConfigurationApp.ExchangeName);
            queueServerService.MessageReceived += MessageReceived;
            queueServerService.CreateConsumer(ConfigurationApp.MainQueueName);
            //TODO SPIO czyszczenie
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var queueServerService = (IQueueServerService)sender;
            var jsonSerializer = new JSonSerializer();
            var type = Type.GetType(e.ContentType);

            if (type == typeof (CreateUserReq))
            {
                var createUserRequest = jsonSerializer.Deserialize<CreateUserReq>(e.Message);
                var queueServerForClient = new RabbitMqServerService(new RabbitMqConnection());
                queueServerForClient.Initialize(ConfigurationApp.Host, ConfigurationApp.UserName, ConfigurationApp.Password, ConfigurationApp.ExchangeName);
                queueServerForClient.CreateQueueForClient(string.Format("archive.{0}", createUserRequest.Login));
                return;
            }

            if (type == typeof (AuthRequest))
            {
                var authRequest = jsonSerializer.Deserialize<AuthRequest>(e.Message);

                if (!_currentUsers.Contains(authRequest.Login))
                {
                    var topicList = new List<string> {e.TopicSender};
                    _currentUsers.Add(authRequest.Login, topicList);
                }
                else
                {
                    var topicList = (ICollection<string>)_currentUsers[authRequest.Login];
                    if (!topicList.Contains(e.TopicSender))
                    {
                        topicList.Add(e.TopicSender);
                    }
                }

                var queueServerForClient = new RabbitMqServerService(new RabbitMqConnection());
                queueServerForClient.Initialize(ConfigurationApp.Host, ConfigurationApp.UserName, ConfigurationApp.Password, ConfigurationApp.ExchangeName);
                var consumer = queueServerForClient.CreateConsumerForClient(string.Format("archive.{0}", authRequest.Login));
                while(true)
                {
                    BasicDeliverEventArgs basicDeliverEventArgs;
                    consumer.Queue.Dequeue(1000, out basicDeliverEventArgs);

                    if (basicDeliverEventArgs == null)
                    {
                        break;
                    }

                    queueServerService.SendData(String.Format("client.{0}", authRequest.Login), basicDeliverEventArgs.Body);
                }
                return;
            }

            if (type == typeof (MessageReq))
            {
                var msgRequest = jsonSerializer.Deserialize<MessageReq>(e.Message);
                bool userInstanceExists = false;
                //TODO spr czy ta osoba jest zarejestrowana
                if (_currentUsers.Contains(msgRequest.Recipient))
                {
                    var topicList = (ICollection<string>) _currentUsers[msgRequest.Recipient];

                    if (topicList.Any())
                    {
                        queueServerService.SendData(String.Format("client.{0}", msgRequest.Recipient), Encoding.UTF8.GetBytes(msgRequest.Message));
                        userInstanceExists=true;
                    }
                }

                if (!userInstanceExists)
                {
                    queueServerService.SendData(String.Format("archive.{0}", msgRequest.Recipient), Encoding.UTF8.GetBytes(msgRequest.Message));
                }
                return;
            }
        }

        public void Stop()
        {
        }
    }
}
