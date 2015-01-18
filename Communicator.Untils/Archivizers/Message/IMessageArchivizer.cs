using System.Collections.Generic;
using Communicator.Protocol.Notifications;
using Communicator.Protocol.Requests;

namespace Communicator.Untils.Archivizers.Message
{
    public interface IMessageArchivizer
    {
        void Save(MessageReq obj, string path);
        List<MessageNotification> Read(string sender, string pathToArchivize);
    }
}
