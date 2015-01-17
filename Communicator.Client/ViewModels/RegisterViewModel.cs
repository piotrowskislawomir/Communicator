using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Communicator.BusinessLayer;
using Communicator.BusinessLayer.Enums;
using Communicator.BusinessLayer.Interfaces;
using Communicator.BusinessLayer.Models;
using Communicator.Client.Annotations;
using Communicator.Client.Models;
using Communicator.Protocol.Model;
using Communicator.Queue.Interfaces;
using Microsoft.Practices.Prism.Commands;

namespace Communicator.Client.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly ILogicClient _logicClient;

        public UserModel User { get; set; }

        public ICommand CreateAccountCommand
        {
            get { return new DelegateCommand(CreateAccount); }
        }

        private void CreateAccount()
        {
            _logicClient.RegisterUser(User);
        }

        public RegisterViewModel(ILogicClient logicClient)
        {
            _logicClient = logicClient;
            _logicClient.Repeater += ProceedCommand;
            User = new UserModel();
        }


        public string Login
        {
            get { return User.Login; }
            set
            {
                User.Login = value;
                OnPropertyChanged();
            }

        }

        public string Password
        {
            get { return User.Password; }
            set
            {
                User.Password = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmedPassword
        {
            get{return User.ConfirmedPassword;}
            set
            {
                User.ConfirmedPassword = value;
                OnPropertyChanged();
            }
        }

        public void ProceedCommand(object sender, RepeaterEventArgs e)
        {
            if (e.Type == ActionTypes.UserCreate)
            {
                MessageBox.Show("Zarejestrowano");
            }
        }
    }
}
