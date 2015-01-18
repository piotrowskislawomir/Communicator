using System;

namespace Communicator.Untils.Archivizers.Message
{
    internal class Message
    {
        public string Sender { get; set; } //nadawca
        public DateTimeOffset DateTimeDelivery { get; set; }
        // generowane na serwerze po odebraniu z kolejki (czas lokalny serwera)
        public string Body { get; set; }
        public string Recipient { get; set; } //odbiorca
    }
}