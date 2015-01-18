using System;
using Communicator.Protocol.Base;
using Communicator.Protocol.Model;

namespace Communicator.Protocol.Requests
{
    public class MessageReq : Request
    {
        public string Recipient { get; set; }
        public string Message { get; set; }
        public Attachment Attachment { get; set; }
        //Server
        public DateTimeOffset SendTime { get; set; }
    }
}