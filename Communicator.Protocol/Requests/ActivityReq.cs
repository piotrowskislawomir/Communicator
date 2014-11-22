using Communicator.Protocol.Base;

namespace Communicator.Protocol.Requests
{
    public class ActivityReq : Request
    {
        public bool IsWriting { get; set; }
        public string Recipient { get; set; }
    }
}
