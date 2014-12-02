using Communicator.Queue.Interfaces;
using RabbitMQ.Client;

namespace Communicator.Queue.Services
{
    public class RabbitMqConnection:IQueueConnection
    {
        public IModel CreateModel(string hostName, string userName, string password, string exchangeType)
        {
            var factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };
            IConnection conn = factory.CreateConnection();
            IModel model = conn.CreateModel();
            return model;
        }
    }
}
