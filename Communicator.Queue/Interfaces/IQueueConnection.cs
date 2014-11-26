using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Communicator.Queue.Interfaces
{
    public interface IQueueConnection
    {
        IModel CreateModel(string hostName, string userName, string password);
    }
}
