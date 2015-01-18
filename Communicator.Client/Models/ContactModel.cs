using Communicator.Protocol.Enums;
using Microsoft.Practices.Prism.Mvvm;

namespace Communicator.Client.Models
{
    public class ContactModel : BindableBase
    {
        //public string Login { get; set; }
        //public PresenceStatus Status { get; set; }

        private string _login;

        private PresenceStatus _status;

        private string _statusImageUri;

        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }

        public PresenceStatus Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public string StatusImageUri
        {
            get { return _statusImageUri; }
            set { SetProperty(ref _statusImageUri, value); }
        }
    }
}