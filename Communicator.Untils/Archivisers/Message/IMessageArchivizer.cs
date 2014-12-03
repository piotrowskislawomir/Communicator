using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator.Untils.Archivisers.Message
{
    // w razie zapisu do innych formatów lub bazy danych
    interface IMessageArchivizer
    {
            void Save(Message obj);
    }
}
