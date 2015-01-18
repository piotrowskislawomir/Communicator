using Communicator.Protocol.Base;

namespace Communicator.Protocol.Requests
{
    public class CreateUserReq : Request
    {
        public string Password { get; set; }
    }
}