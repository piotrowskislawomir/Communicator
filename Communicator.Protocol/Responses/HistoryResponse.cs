using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Protocol.Base;
using Communicator.Protocol.Notifications;

namespace Communicator.Protocol.Responses
{
    public class HistoryResponse: Response
    {
        public List<MessageNotification> Messages { get; set; }
    }
}
