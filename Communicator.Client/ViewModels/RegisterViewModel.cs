using System;
using System.Windows.Input;
using Communicator.BusinessLayer;
using Communicator.BusinessLayer.Enums;
using Communicator.BusinessLayer.Interfaces;
using Communicator.BusinessLayer.Models;
using Microsoft.Practices.Prism.Commands;

namespace Communicator.Client.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly ILogicClient _logicClient;
        private string _result;

        public RegisterViewModel(ILogicClient logicClient)
        {
            _logicClient = logicClient;
            _logicClient.Repeater += ProceedCommand;
            User = new UserModel();
        }

        public UserModel User { get; set; }

        public ICommand CreateAccountCommand
        {
            get { return new DelegateCommand(CreateAccount); }
        }

        public ICommand CloseCommand
        {
            get { return new DelegateCommand(CloseAction); }
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
            get { return User.ConfirmedPassword; }
            set
            {
                User.ConfirmedPassword = value;
                OnPropertyChanged();
            }
        }

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler OnRequestClose;

        private void CloseAction()
        {
            if (OnRequestClose != null)
            {
                OnRequestClose(this, new EventArgs());
            }
        }

        private void CreateAccount()
        {
            if (Password != ConfirmedPassword)
            {
                Result = "Hasła nie są identyczne";
                return;
            }

            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                Result = "Uzupełnij wymagane pola";
                return;
            }

            _logicClient.RegisterUser(User);
            Result = string.Empty;
        }

        public void ProceedCommand(object sender, RepeaterEventArgs e)
        {
            if (e.Type == ActionTypes.UserCreate)
            {
                if (e.Result)
                {
                    Result = "Zarejestrowano";
                    Login = string.Empty;
                    Password = string.Empty;
                    ConfirmedPassword = string.Empty;
                }
                else
                {
                    Result = "Błąd podczas rejestracji";
                }
            }
        }
    }
}