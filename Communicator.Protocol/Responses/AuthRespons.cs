using Communicator.Protocol.Base;

namespace Communicator.Protocol.Responses
{
    public class AuthResponse : Response
    {
        public bool IsAuthenticated { get; set; }
    }
}