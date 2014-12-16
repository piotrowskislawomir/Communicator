using Communicator.Client.Models;
using Communicator.Protocol.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator.Client.ViewModels
{
    public class ContactViewModel
    {
        public List<ContactModel> ContactList { get; set; }

        public ContactViewModel()
        {
            ContactList = new List<ContactModel>{
                new ContactModel{
                    Login = "John",
                    Status = PresenceStatus.Offline
                },
                new ContactModel{
                    Login = "Doe",
                    Status = PresenceStatus.Online
                },
                new ContactModel{
                    Login = "JohnDoe",
                    Status = PresenceStatus.Offline
                }
            };

        }
    }
}
