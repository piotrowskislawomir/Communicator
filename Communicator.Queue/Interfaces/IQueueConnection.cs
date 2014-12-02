using RabbitMQ.Client;

namespace Communicator.Queue.Interfaces
{
    public interface IQueueConnection
    {
        IModel CreateModel(string hostName, string userName, string password, string exchangeType);
    }
}
