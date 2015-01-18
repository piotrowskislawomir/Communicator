using System;
using System.Windows.Input;
using Communicator.BusinessLayer;
using Communicator.BusinessLayer.Enums;
using Communicator.BusinessLayer.Interfaces;
using Communicator.BusinessLayer.Models;
using Communicator.Client.Helpers;
using Microsoft.Practices.Prism.Commands;

namespace Communicator.Client.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ILogicClient _logicClient;
        private string _result;

        public LoginViewModel(ILogicClient logicClient)
        {
            _logicClient = logicClient;
            User = new UserModel();
            _logicClient.Repeater += ProceedCommand;
        }

        public ICommand LoginCommand
        {
            get { return new DelegateCommand(LoginAction); }
        }

        public ICommand RegisterCommand
        {
            get { return new DelegateCommand(RegisterAction); }
        }

        public ICommand CloseCommand
        {
            get { return new DelegateCommand(CloseAction); }
        }

        public UserModel User { get; set; }

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

        private void RegisterAction()
        {
            var registerWindow = new RegisterWindow();
            var registerViewModel = new RegisterViewModel(_logicClient);
            registerViewModel.OnRequestClose += (s, e) => registerWindow.Close();
            registerWindow.DataContext = registerViewModel;
            registerWindow.Show();
        }

        private void LoginAction()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                Result = "Uzupełnij wymagane pola";
                return;
            }

            _logicClient.LoginUser(User);
            Result = string.Empty;
        }

        public void ProceedCommand(object sender, RepeaterEventArgs e)
        {
            if (e.Type == ActionTypes.Login)
            {
                if (e.Result)
                {
                    DispatchService.Invoke(() =>
                    {
                        _logicClient.Login = Login;
                        var communicatorWindow = new CommunicatorWindow();
                        var communicatorViewModel = new CommunicatorViewModel(_logicClient);
                        communicatorViewModel.OnRequestClose += (s, ee) => communicatorWindow.Close();
                        communicatorWindow.DataContext = communicatorViewModel;
                        communicatorWindow.Show();
                        communicatorViewModel.Inicialize();

                        if (OnRequestClose != null)
                        {
                            OnRequestClose(this, new EventArgs());
                        }
                    });
                }
                else
                {
                    Result = "Niepoprawne hało lub login";
                }
            }
        }
    }
}