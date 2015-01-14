using Communicator.Client.Models;
using Communicator.Protocol.Enums;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Communicator.Client.ViewModels
{
    public class ContactsViewModel
    {
        public ObservableCollection<ContactViewModel> Contacts { get; set; }

        private static string avaible = "../UI/statusGreen.jpg";
        private static string unavaible = "../UI/statusRed.jpg";

        public ContactsViewModel()
        {
            Contacts = new ObservableCollection<ContactViewModel>{
                new ContactViewModel(new ContactModel{
                    Login = "John",
                    Status = PresenceStatus.Online,
                    StatusImageUri = avaible
                }),
                new ContactViewModel(new ContactModel{
                    Login = "Doe",
                    Status = PresenceStatus.Offline,
                    StatusImageUri = unavaible
                }),
                new ContactViewModel(new ContactModel{
                    Login = "JohnDoe",
                    Status = PresenceStatus.Offline,
                    StatusImageUri = unavaible
                }),
            };

            ChangeStatusCommand = new DelegateCommand<string>(OnStatusChange);
        }

        public ICommand ChangeStatusCommand { get; set; }


        private void OnStatusChange(string userName)
        {
            //sprawdź login dla obecnego użytkownika
            var user = Contacts.FirstOrDefault(c => c.Login == userName);

            if (user != null)
            {
                //sprawdz status i zwroc odpowiedni link do obrazka 
                user.StatusImageUri = user.StatusImageUri == unavaible ? avaible : unavaible;
                //if (user.StatusImageUri == unavaible)
                //    user.StatusImageUri = avaible;
                //else
                //    user.StatusImageUri = unavaible;
            }
        }



    }
}
