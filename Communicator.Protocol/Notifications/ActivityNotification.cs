using Communicator.Protocol.Base;

namespace Communicator.Protocol.Notifications
{
    public class ActivityNotification : Notification
    {
        public string Sender { get; set; }
        public bool IsWriting { get; set; }
    }
}