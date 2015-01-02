using Communicator.Protocol.Enums;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Communicator.Client.Models
{
    public class ContactModel : BindableBase
    {
        //public string Login { get; set; }
        //public PresenceStatus Status { get; set; }

        string _login;
        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }

        PresenceStatus _status;
        public PresenceStatus Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        string _statusImageUri;
        public string StatusImageUri
        {
            get { return _statusImageUri; }
            set 
            { 
                SetProperty(ref _statusImageUri, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string name = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
