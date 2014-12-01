using Communicator.Client.Models;
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
                    Name = "Jakiś1",
                    Nick = "John",
                    Status = 1
                },
                new ContactModel{
                    Name = "Jakiś2",
                    Nick = "Doe",
                    Status = 1
                },
                new ContactModel{
                    Name = "Jakiś3",
                    Nick = "JohnDoe",
                    Status = 1
                }
            };

        }
    }
}
