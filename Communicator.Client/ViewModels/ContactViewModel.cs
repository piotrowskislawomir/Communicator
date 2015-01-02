using Communicator.Client.Models;
using Communicator.Protocol.Enums;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Communicator.Client.ViewModels
{
    public class ContactViewModel 
    {
        public ObservableCollection<ContactModel> Contact { get; set; }

        private static string avaible = "../UI/statusGreen.jpg";
        private static string unavaible = "../UI/statusRed.jpg";

        public ContactViewModel()
        {
            Contact = new ObservableCollection<ContactModel>{
                new ContactModel{
                    Login = "John",
                    Status = PresenceStatus.Offline,
                    StatusImageUri = unavaible
                },
                new ContactModel{
                    Login = "Doe",
                    Status = PresenceStatus.Online,
                    StatusImageUri = avaible
                },
                new ContactModel{
                    Login = "JohnDoe",
                    Status = PresenceStatus.Offline,
                    StatusImageUri = unavaible
                }
            };

            ChangeStatusCommand = new DelegateCommand<string>(OnStatusChange);
        }
        
        public ICommand ChangeStatusCommand { get; set; }


        private void OnStatusChange(string userName)
        {
            //sprawdź login dla obecnego użytkownika
            ContactModel user = Contact.Where(c => c.Login == userName).FirstOrDefault();
            
            //sprawdz status i zwroc odpowiedni link do obrazka 
            if (user.StatusImageUri == unavaible)
                user.StatusImageUri = avaible;
            else
                user.StatusImageUri = unavaible;
        }

    }
}
