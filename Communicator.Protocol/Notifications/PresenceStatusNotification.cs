using Communicator.Protocol.Base;
using Communicator.Protocol.Enums;

namespace Communicator.Protocol.Notifications
{
    public class PresenceStatusNotification : Notification
    {
        public string Login { get; set; }
        public PresenceStatus PresenceStatus { get; set; }
    }
}
