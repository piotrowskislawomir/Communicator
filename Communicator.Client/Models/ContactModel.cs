using Communicator.Protocol.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator.Client.Models
{
    public class ContactModel
    {
        public string Login { get; set; }
        public PresenceStatus Status { get; set; }

    }
}
