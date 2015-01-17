using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Protocol.Model;
using Communicator.Protocol.Requests;

namespace Communicator.Untils.Archivizers.Message
{
    // w razie zapisu do innych formatów lub bazy danych
    interface IMessageArchivizer
    {
            void Save(MessageReq obj, string path);
            List<MessageNotification> Read(User user, string path);
    }
}
