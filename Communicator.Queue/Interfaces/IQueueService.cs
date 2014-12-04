using Communicator.Queue.Services;

namespace Communicator.Queue.Interfaces
{
    public interface IQueueService
    {
        event MessageReceivedEventHandler MessageReceived;
        void Initialize(string host, string userName, string password, string exchangeName);
        void CreateConsumer(string routingKey);
       
    }
}
