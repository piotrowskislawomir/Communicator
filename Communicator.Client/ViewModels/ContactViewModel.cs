using Communicator.Client.Models;
using Communicator.Protocol.Enums;

namespace Communicator.Client.ViewModels
{
    public class ContactViewModel : ViewModelBase
    {
        private readonly ContactModel _contact;

        public ContactViewModel(ContactModel c)
        {
            _contact = c;
        }

        public PresenceStatus Status
        {
            get { return _contact.Status; }
            set
            {
                _contact.Status = value;
                OnPropertyChanged();
            }
        }

        public string Login
        {
            get { return _contact.Login; }
            set
            {
                _contact.Login = value;
                OnPropertyChanged();
            }
        }

        public string StatusImageUri
        {
            get { return _contact.StatusImageUri; }
            set
            {
                _contact.StatusImageUri = value;
                OnPropertyChanged();
            }
        }
    }
}