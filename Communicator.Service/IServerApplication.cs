using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator.Server
{
    public interface IServerApplication
    {
        void Start();
        void Stop();
    }
}
