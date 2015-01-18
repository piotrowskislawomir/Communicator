using RabbitMQ.Client;

namespace Communicator.Queue.Interfaces
{
    public interface IQueueManagerService
    {
        void Initialize(string host, string userName, string password, string exchangeName);
        void CreateQueue(string queueName, string exchangeName);
        QueueingBasicConsumer CreateConsumerForClient(string queueName);
        void SendAck(ulong deliveryTag);
    }
}