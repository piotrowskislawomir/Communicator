using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ.Events;

namespace Communicator.Client.Models
{
    public class MessageModel
    {
        public string Sender { get; set; } //nadawca
        public string DateTimeDelivery { get; set; }
        public string Message { get; set; }
        public string Recipient { get; set; } //odbiorca

        public object Image { get; set; }
    }
}
