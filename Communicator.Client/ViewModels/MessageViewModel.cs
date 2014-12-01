using Communicator.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator.Client.ViewModels
{
    public class MessageViewModel
    {
        public List<MessageModel> MessagesList { get; set; }

        public MessageViewModel()
        {
            MessagesList = new List<MessageModel>{
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

        }
    }
}
