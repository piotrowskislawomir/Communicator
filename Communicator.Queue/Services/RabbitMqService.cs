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
    public class RabbitMqService : IQueueService
    {
        private readonly IQueueConnection _queueConnection;
        private IModel _model;
        public RabbitMqService()
        {
            _queueConnection = new RabbitMqConnection();
        }
        public RabbitMqService(IQueueConnection queueConnection)
        {
            _queueConnection = queueConnection;
        }

        public void Initialize()
        {
            _model = _queueConnection.CreateModel(ConfigurationApp.Host, ConfigurationApp.UserName, ConfigurationApp.Password);
        }

        public void CreateConsumer(string key)
        {
            var queue = _model.QueueDeclare();
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received +=
                (_, msg) => Console.WriteLine("Consumer received message {0}", Encoding.UTF8.GetString(msg.Body));
            _model.QueueBind(queue.QueueName, ConfigurationApp.ExchangeName, key);
            _model.BasicConsume(queue.QueueName, false, consumer);
        }

        public void SendData(string key)
        {
            _model.BasicPublish(ConfigurationApp.ExchangeName, key, _model.CreateBasicProperties(), Encoding.UTF32.GetBytes("widomows"));

        }

    }
}
