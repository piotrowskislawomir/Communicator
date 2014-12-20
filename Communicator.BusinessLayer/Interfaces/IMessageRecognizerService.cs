using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Queue;
using Communicator.Queue.Interfaces;
using Communicator.Untils.Configuration;

namespace Communicator.BusinessLayer.Interfaces
{
    public interface IMessageRecognizerService
    {
        IConfigurationService ConfigurationService { get; set; }
        IQueueServerService QueueServerService { get; set; }
        void ProcessMessage(MessageReceivedEventArgs messageReceivedEventArgs);
    }
}
