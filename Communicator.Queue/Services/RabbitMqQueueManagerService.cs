using Communicator.Queue.Interfaces;
using RabbitMQ.Client;

namespace Communicator.Queue.Services
{
    public class RabbitMqQueueManagerService : IQueueManagerService
    {
        private readonly IQueueConnection _queueConnection;
        private IModel _model;

        public RabbitMqQueueManagerService(IQueueConnection queueConnection)
        {
            _queueConnection = queueConnection;
        }

        public void Initialize(string host, string userName, string password, string exchangeName)
        {
            _model = _queueConnection.CreateModel(host, userName, password, ExchangeType.Topic);
            _model.ExchangeDeclare(exchangeName, ExchangeType.Topic);
        }

        public void CreateQueue(string queueName, string exchangeName)
        {
            QueueDeclareOk queue = _model.QueueDeclare(queueName, true, false, false, null);
            _model.QueueBind(queue.QueueName, exchangeName, queueName);
        }

        public QueueingBasicConsumer CreateConsumerForClient(string queueName)
        {
            var consumer = new QueueingBasicConsumer(_model);
            _model.BasicConsume(queueName, false, consumer);
            return consumer;
        }

        public void SendAck(ulong deliveryTag)
        {
            _model.BasicAck(deliveryTag, false);
        }
    }
}