using Communicator.Queue;
using Communicator.Queue.Interfaces;
using Communicator.Untils.Interfaces;

namespace Communicator.BusinessLayer.Interfaces
{
    public interface IMessageRecognizerService
    {
        IConfigurationService ConfigurationService { get; set; }
        IQueueServerService QueueServerService { get; set; }
        void ProcessMessage(MessageReceivedEventArgs messageReceivedEventArgs);
        void Initialize();
    }
}