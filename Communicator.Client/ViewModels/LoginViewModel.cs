using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Communicator.BusinessLayer;
using Communicator.BusinessLayer.Enums;
using Communicator.BusinessLayer.Interfaces;
using Communicator.BusinessLayer.Models;
using Communicator.Client.Annotations;
using Communicator.Protocol.Model;
using Microsoft.Practices.Prism.Commands;

namespace Communicator.Client.ViewModels
{
    public class LoginViewModel:INotifyPropertyChanged
    {
        private readonly ILogicClient _logicClient;
        public event PropertyChangedEventHandler PropertyChanged;

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

        public LoginViewModel(ILogicClient logicClient)
        {
            _logicClient = logicClient;
            User =new UserModel();
            _logicClient.Repeater += ProceedCommand;
        }

        private void RegisterAction()
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
        }

        private void LoginAction()
        {
            _logicClient.LoginUser(User);
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

        public void ProceedCommand(object sender, RepeaterEventArgs e)
        {
            if (e.Type == ActionTypes.Login)
            {
                MessageBox.Show("Logowanie: " + e.Result);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
