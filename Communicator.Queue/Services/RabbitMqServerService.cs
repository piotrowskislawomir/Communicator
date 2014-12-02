using Communicator.Queue.Interfaces;
using Communicator.Untils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Communicator.Queue.Services
{
    public class RabbitMqServerService: IQueueServerService
    {
        public event MessageReceivedEventHandler MessageReceived;
        private readonly IQueueConnection _queueConnection;
        private IModel _model;

        public RabbitMqServerService(IQueueConnection queueConnection)
        {
            _queueConnection = queueConnection;
        }

        public void Initialize()
        {
            _model = _queueConnection.CreateModel(ConfigurationApp.Host, ConfigurationApp.UserName, ConfigurationApp.Password, ExchangeType.Direct);
            _model.ExchangeDeclare(ConfigurationApp.ExchangeName, ExchangeType.Topic);
        }

        public void CreateConsumer(string queueName)
        {
            var queue = _model.QueueDeclare(ConfigurationApp.MainQueueName, true, false, false,null);
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received +=
                (_, msg) =>
                {
                    //TODO SPIO
                    _model.BasicAck(msg.DeliveryTag, false);
                };
            _model.QueueBind(queue.QueueName, ConfigurationApp.ExchangeName, queue.QueueName);
            _model.BasicConsume(queue.QueueName, false, consumer);
        }

        public void SendData(string routingKey, byte[] data)
        {
            var properties = _model.CreateBasicProperties();
            properties.SetPersistent(true);

            _model.BasicPublish(ConfigurationApp.ExchangeName, routingKey, properties, data);
        }

    }
}
