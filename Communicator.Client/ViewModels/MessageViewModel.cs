using Communicator.Client.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Communicator.Client.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        private ObservableCollection<MessageModel> _messages;
        public ObservableCollection<MessageModel> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        public MessageViewModel()
        {
            _messages = new ObservableCollection<MessageModel>{
                new MessageModel{
                    Sender = "Ja",
                    Recipient = "John",
                    Message = "jakaś tam sobie wiadomość"
                },
                new MessageModel{
                    Sender = "Ja",
                    Recipient = "Doe",
                    Message = "jakaś treść wiadomości"
                },
                new MessageModel{
                    Sender = "Ja",
                    Recipient = "JohnDoe",
                    Message = "nowa wiadomość"
                }
            };

            AddMessageCommand = new DelegateCommand<string>(OnMessageAdd);
        }

        public ICommand AddMessageCommand { get; set; }


        private void OnMessageAdd(string message)
        {
            //dodaj do listy wiadomosci wraz z info o nadawcy, czasie, tresci wiadomosci
            _messages.Add(new MessageModel
            {
                DateTimeDelivery = DateTimeOffset.Now,
                Message = message,
                Sender = "Ja",
                Recipient = "JohnDoe"
            });
        }
    }
}
