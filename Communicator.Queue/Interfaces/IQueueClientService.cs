using System;

namespace Communicator.Queue.Interfaces
{
    public interface IQueueClientService:IQueueService
    {
        void SendData(string queueName, string routingKey, byte[] data, Type dataType);
    }
}
