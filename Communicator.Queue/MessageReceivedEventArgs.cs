namespace Communicator.Queue
{
    public class MessageReceivedEventArgs
    {
        public byte[] Message { get; set; }
        public string ContentType { get; set; }
        public string TopicSender { get; set; }
    }
}
