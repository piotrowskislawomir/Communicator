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
            var queueServerService = sender as IQueueServerService;
            var jsonSerializer = new JSonSerializer();
            var type = Type.GetType(e.ContentType);

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
            }

            if (type == typeof (MessageReq))
            {
                var msgRequest = jsonSerializer.Deserialize<MessageReq>(e.Message);

                if (_currentUsers.Contains(msgRequest.Recipient))
                {
                    if (queueServerService != null)
                    {
                        var topicList = (ICollection<string>) _currentUsers[msgRequest.Recipient];
                        topicList.ToList()
                            .ForEach(t => queueServerService.SendData(t, Encoding.UTF8.GetBytes(msgRequest.Message)));

                    }
                }
            }
        }

        public void Stop()
        {
        }
    }
}
