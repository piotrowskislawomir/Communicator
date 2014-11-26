using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Queue.Interfaces;
using Communicator.Untils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Communicator.Queue.Services
{
    public class RabbitMqConnection:IQueueConnection
    {
        public IModel CreateModel(string hostName, string userName, string password)
        {
            var factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };
            IConnection conn = factory.CreateConnection();
            IModel model = conn.CreateModel();
            model.ExchangeDeclare(ConfigurationApp.ExchangeName, ExchangeType.Topic);
            return model;
        }
    }
}
