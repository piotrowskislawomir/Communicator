namespace Communicator.Protocol.Model
{
    public class Attachment
    {
        public byte[] Data { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
    }
}