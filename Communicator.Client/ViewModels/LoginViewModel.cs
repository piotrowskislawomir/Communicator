using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Communicator.BusinessLayer;
using Communicator.BusinessLayer.Enums;
using Communicator.BusinessLayer.Interfaces;
using Communicator.BusinessLayer.Models;
using Communicator.Client.Annotations;
using Communicator.Client.Helpers;
using Communicator.Protocol.Model;
using Microsoft.Practices.Prism.Commands;

namespace Communicator.Client.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ILogicClient _logicClient;
        private string _result;

        public event EventHandler OnRequestClose;
        public ICommand LoginCommand
        {
            get
            {
                return new DelegateCommand(LoginAction);
            }
        }

        public ICommand RegisterCommand
        {
            get
            {
                return new DelegateCommand(RegisterAction);
            }
        }

        public ICommand CloseCommand
        {
            get { return new DelegateCommand(CloseAction); }
        }

        private void CloseAction()
        {
            if (OnRequestClose != null)
            {
                OnRequestClose(this, new EventArgs());
            }
        }

        public LoginViewModel(ILogicClient logicClient)
        {
            _logicClient = logicClient;
            User = new UserModel();
            _logicClient.Repeater += ProceedCommand;
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
