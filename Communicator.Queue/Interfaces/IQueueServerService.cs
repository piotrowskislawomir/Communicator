using RabbitMQ.Client;

namespace Communicator.Queue.Interfaces
{
    public interface IQueueServerService : IQueueService
    {
        void SendData<T>(string routingKey, string exchangeName, T data);
    }
}
