using System;
using System.Text;
using Communicator.Queue.Interfaces;
using Communicator.Untils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Communicator.Queue.Services
{
    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

    public class RabbitMqClientService : IQueueClientService
    {
        private readonly IQueueConnection _queueConnection;
        private IModel _model;

        public event MessageReceivedEventHandler MessageReceived;

        public RabbitMqClientService(IQueueConnection queueConnection)
        {
            _queueConnection = queueConnection;
        }

        public string GetUniqueTopic(string login)
        {
            return String.Format("client.{0}.{1}.{2}",Environment.MachineName,login,Guid.NewGuid());
        }

        public void Initialize(string host, string userName, string password, string exchangeName)
        {
            _model = _queueConnection.CreateModel(host, userName, password, ExchangeType.Direct);
        }

        public void CreateConsumer(string routingKey)
        {
            var queue = _model.QueueDeclare();
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received +=
                (_, msg) =>
                {
                    MessageReceived(this, CreateMessage(msg));
                    _model.BasicAck(msg.DeliveryTag, false);
                };
            _model.QueueBind(queue.QueueName, ConfigurationApp.ExchangeName, routingKey);
            _model.BasicConsume(queue.QueueName, false, consumer);
        }

        private MessageReceivedEventArgs CreateMessage(BasicDeliverEventArgs e)
        {
           var msg = new MessageReceivedEventArgs {Message = Encoding.UTF8.GetString(e.Body)};
            return msg;
        }

        public void SendData(string queueName,string routingKey, byte[] data, Type dataType)
        {
            var properties = _model.CreateBasicProperties();
            properties.SetPersistent(true);
            properties.ReplyTo = routingKey;
            properties.Type = dataType.AssemblyQualifiedName;

            _model.BasicPublish(ConfigurationApp.ExchangeName, queueName, properties, data);
        }

    }

}
