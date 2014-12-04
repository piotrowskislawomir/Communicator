namespace Communicator.Queue
{
    public class MessageReceivedEventArgs
    {
        public string Message { get; set; }
        public string ContentType { get; set; }
        public string Sender { get; set; }
    }
}
