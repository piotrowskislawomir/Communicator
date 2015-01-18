using System;
using Communicator.Protocol.Model;

namespace Communicator.Protocol.Notifications
{
    public class MessageNotification
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Message { get; set; }
        public Attachment Attachment { get; set; }
        public DateTimeOffset SendTime { get; set; }
    }
}