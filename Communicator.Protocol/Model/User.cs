using Communicator.Protocol.Enums;

namespace Communicator.Protocol.Model
{
    public class User
    {
        public string Login { get; set; }
        public PresenceStatus Status { get; set; }
    }
}