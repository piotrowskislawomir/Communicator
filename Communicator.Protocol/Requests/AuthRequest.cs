using Communicator.Protocol.Base;

namespace Communicator.Protocol.Requests
{
    public class AuthRequest : Request
    {
        public string Password { get; set; }
    }
}
