namespace Communicator.Queue.Interfaces
{
    public interface IQueueClientService:IQueueService
    {
        void SendData<T>(string queueName, string routingKey, string exchangeName, T data);

    }
}
