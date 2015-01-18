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