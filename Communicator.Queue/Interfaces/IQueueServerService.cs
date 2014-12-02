namespace Communicator.Queue.Interfaces
{
    public interface IQueueServerService :IQueueService
    {
        void SendData(string routingKey, byte[] data);
    }
}
