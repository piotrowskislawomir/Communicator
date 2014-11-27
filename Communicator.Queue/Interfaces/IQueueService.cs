using Communicator.Queue.Services;

namespace Communicator.Queue.Interfaces
{
    public interface IQueueService
    {
        event MessageReceivedEventHandler MessageReceived;
        void Initialize();
        void CreateConsumer(string key);
        void SendData(string key, byte[] data);
    }
}
