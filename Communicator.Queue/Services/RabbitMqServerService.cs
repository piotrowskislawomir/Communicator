using Communicator.Queue.Interfaces;
using Communicator.Untils.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Communicator.Queue.Services
{
    public class RabbitMqServerService : IQueueServerService
    {
        public event MessageReceivedEventHandler MessageReceived;
        private readonly IQueueConnection _queueConnection;
        private readonly ISerializerService _serializerService;
        private IModel _model;

        public RabbitMqServerService(IQueueConnection queueConnection, ISerializerService serializerService)
        {
            _queueConnection = queueConnection;
            _serializerService = serializerService;
        }

        public void Initialize(string host, string userName, string password, string exchangeName)
        {
            _model = _queueConnection.CreateModel(host, userName, password, exchangeName);
            _model.ExchangeDeclare(exchangeName, ExchangeType.Topic);
        }

        public void CreateConsumer(string queueName, string exchangeName)
        {
            var queue = _model.QueueDeclare(queueName, true, false, false, null);
            var consumer = new EventingBasicConsumer(_model);

            consumer.Received +=
                (_, msg) =>
                {
                    MessageReceived(this, new MessageReceivedEventArgs()
                    {
                        Message = msg.Body,
                        ContentType = msg.BasicProperties.Type,
                        TopicSender = msg.BasicProperties.ReplyTo
                    });

                    _model.BasicAck(msg.DeliveryTag, false);
                };
            _model.QueueBind(queue.QueueName, exchangeName, queue.QueueName);
            _model.BasicConsume(queue.QueueName, false, consumer);
        }

        public void SendData<T>(string routingKey, string exchangeName, T data)
        {
            var properties = _model.CreateBasicProperties();
            properties.SetPersistent(true);
            properties.ReplyTo = routingKey;
            properties.Type = typeof(T).AssemblyQualifiedName;

            byte[] buffer = _serializerService.Serialize(data);
            _model.BasicPublish(exchangeName, routingKey, properties, buffer);
        }

    }

}
