using System.Collections.Generic;
using Communicator.Protocol.Base;
using Communicator.Protocol.Notifications;

namespace Communicator.Protocol.Responses
{
    public class HistoryResponse : Response
    {
        public List<MessageNotification> Messages { get; set; }
    }
}