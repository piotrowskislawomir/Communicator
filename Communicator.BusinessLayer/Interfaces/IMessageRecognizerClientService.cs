using Communicator.Queue;

namespace Communicator.BusinessLayer.Interfaces
{
    public interface IMessageRecognizerClientService
    {
        event RepeaterEventHandler Repeater;
        void ProceedMessage(MessageReceivedEventArgs e);
    }
}