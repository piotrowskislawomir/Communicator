using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Communicator.BusinessLayer;
using Communicator.BusinessLayer.Enums;
using Communicator.BusinessLayer.Interfaces;
using Communicator.Client.Helpers;
using Communicator.Client.Models;
using Communicator.Protocol.Model;
using Communicator.Protocol.Notifications;
using Communicator.Protocol.Responses;
using Microsoft.Practices.Prism.Commands;

namespace Communicator.Client.ViewModels
{
    public class HistoryViewModel:ViewModelBase
    {
        private readonly ILogicClient _logicClient;
        public event EventHandler OnRequestClose;

        public List<MessageNotification> History { get; set; }

        public ObservableCollection<MessageModel> UserMessages { get; set; }
        public ObservableCollection<User> UserList { get; set; }

        public HistoryViewModel(ILogicClient logicClient)
        {
            _logicClient = logicClient;
            _logicClient.Repeater += ProceedCommand;
            UserMessages = new ObservableCollection<MessageModel>();
            UserList = new ObservableCollection<User>();
            _logicClient.GetHistory();
        }

        public ICommand CloseCommand
        {
            get { return new DelegateCommand(CloseAction); }
        }

        public ICommand UserCommand
        {
            get { return new DelegateCommand<string>(UserAction); }
        }

        private void UserAction(string login)
        {
            var messages = History.Where(m => m.Sender == login || m.Recipient == login);

            UserMessages.Clear();
            foreach (var message in messages.OrderByDescending(m => m.SendTime))
            {
                UserMessages.Add(new MessageModel
                {
                    DateTimeDelivery = message.SendTime.ToString("yy-MM-dd hh:mm"),
                    Message = message.Message,
                    Recipient = message.Recipient,
                    Sender = message.Sender
                });
            }
    }

        private void CloseAction()
        {
            if (OnRequestClose != null)
            {
                OnRequestClose(this, new EventArgs());
            }
        }

        public void ProceedCommand(object sender, RepeaterEventArgs e)
        {
            if (e.Type == ActionTypes.History)
            {
                if (e.Result)
                {
                    var history = (HistoryResponse) e.Data;
                    History = history.Messages;

                    GetAllUsers();
                }
            }
        }

        private void GetAllUsers()
        {
            var users = History.Select(m => m.Sender);
            users = users.Union(History.Select(m => m.Recipient));
            users = users.Where(u => u != _logicClient.Login);

            DispatchService.Invoke(() =>
            {
                UserList.Clear();
                foreach (var user in users)
                {
                    UserList.Add(new User {Login = user});
                }
            });
        }
    }
}
