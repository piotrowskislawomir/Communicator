using System;
using System.Text;
using Communicator.Queue.Interfaces;
using Communicator.Untils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Communicator.Queue.Services
{
    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

    public class RabbitMqService : IQueueService
    {
        private readonly IQueueConnection _queueConnection;
        private IModel _model;

        public event MessageReceivedEventHandler MessageReceived;

        public RabbitMqService(IQueueConnection queueConnection)
        {
            _queueConnection = queueConnection;
        }

        public string GetUniqueTopic()
        {
            return String.Format("client.{0}.{1}",Environment.MachineName,Guid.NewGuid());
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
                (_, msg) =>
                {
                    MessageReceived(this, CreateMessage(msg));
                    _model.BasicAck(msg.DeliveryTag, false);
                };
            _model.QueueBind(queue.QueueName, ConfigurationApp.ExchangeName, key);
            _model.BasicConsume(queue.QueueName, false, consumer);
        }

        private MessageReceivedEventArgs CreateMessage(BasicDeliverEventArgs e)
        {
           var msg = new MessageReceivedEventArgs {Message = Encoding.UTF8.GetString(e.Body)};
            return msg;
        }

        public void SendData(string key, byte[] data)
        {
            var properties = _model.CreateBasicProperties();
            properties.SetPersistent(true);

            _model.BasicPublish(ConfigurationApp.ExchangeName, key, properties, data);
        }

    }
}
