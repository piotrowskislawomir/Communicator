using System.Collections.Generic;
using Communicator.Protocol.Base;
using Communicator.Protocol.Model;

namespace Communicator.Protocol.Responses
{
    public class UserListResponse : Response
    {
        public List<User> Users { get; set; }
    }
}