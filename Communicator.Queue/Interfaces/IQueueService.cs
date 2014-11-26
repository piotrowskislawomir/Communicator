using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Communicator.Queue.Interfaces
{
    public interface IQueueService
    {
        void Initialize();
        IBasicConsumer CreateConsumer(string key);
        void SendData(string key, byte[] data);
    }
}
