using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Communicator.BusinessLayer;
using Communicator.BusinessLayer.Enums;
using Communicator.BusinessLayer.Interfaces;
using Communicator.Client.Annotations;
using Communicator.Client.Helpers;
using Communicator.Client.Models;
using Communicator.Protocol.Requests;
using Microsoft.Practices.Prism.Commands;

namespace Communicator.Client.ViewModels
{
    public class ConversationViewModel : INotifyPropertyChanged
    {
        private readonly ILogicClient _logicClient;
        public event EventHandler OnRequestClose;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MessageModel> Messages { get; set; }
        public string Recipeint { get; set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendCommand
        {
            get
            {
                return new DelegateCommand(SendAction);
            }
        }

        private void SendAction()
        {
            _logicClient.SendMessage(Recipeint,Message);
            Message = string.Empty;
        }

        public ICommand CloseCommand
        {
            get
            {
                return new DelegateCommand(CloseAction);
            }
        }

        private void CloseAction()
        {
            if (OnRequestClose != null)
            {
                OnRequestClose(this, new EventArgs());
            }
        }

        public ConversationViewModel(ILogicClient logicClient)
        {
            _logicClient = logicClient;
            _logicClient.Repeater += ProceedCommand;
            Messages = new ObservableCollection<MessageModel>();
        }

        public void Initialize(string login)
        {
            Recipeint = login;
        }

        public void AddMessage(MessageModel message)
        {
            Messages.Add(message);
        }

        public void ProceedCommand(object sender, RepeaterEventArgs e)
        {
            if (e.Type == ActionTypes.Message)
            {
                if (e.Result)
                {
                    var message = (MessageReq) e.Data;
                    if (message.Login == Recipeint)
                    {
                        var messageModel = new MessageModel
                        {
                            DateTimeDelivery = message.SendTime,
                            Message = message.Message,
                            Sender = message.Login
                        };
                        DispatchService.Invoke(() =>
                        {
                            Messages.Add(messageModel);
                        });
                    }
                }
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
