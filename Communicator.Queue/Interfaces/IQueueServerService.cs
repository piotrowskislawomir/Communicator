using RabbitMQ.Client;

namespace Communicator.Queue.Interfaces
{
    public interface IQueueServerService :IQueueService
    {
        void SendData(string routingKey, byte[] data);
        QueueingBasicConsumer CreateConsumerForClient(string queueName);

        void CreateQueueForClient(string queueName);
    }
}
