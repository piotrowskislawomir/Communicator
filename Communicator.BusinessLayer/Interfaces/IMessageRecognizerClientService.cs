using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Queue;

namespace Communicator.BusinessLayer.Interfaces
{
    public interface IMessageRecognizerClientService
    {
        event RepeaterEventHandler Repeater;
        void ProceedMessage(MessageReceivedEventArgs e);
    }
}
