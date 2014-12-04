using System;
using System.Text;
using System.Threading.Tasks;
using Communicator.Queue;
using Communicator.Queue.Interfaces;
using Communicator.Queue.Services;
using Communicator.Untils;

namespace Communicator.Server
{
    public class ServerApplication
    {

        public void Start()
        {
            Task.Factory.StartNew(CreateNewListener);
        }

        private void CreateNewListener()
        {
            var queueServerService = new RabbitMqServerService(new RabbitMqConnection());
            queueServerService.Initialize(ConfigurationApp.Host, ConfigurationApp.UserName, ConfigurationApp.Password, ConfigurationApp.ExchangeName);
            queueServerService.MessageReceived += MessageReceived;
            queueServerService.CreateConsumer(ConfigurationApp.MainQueueName);

            //TODO SPIO czyszczenie
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Message);
            var queueServerService = sender as IQueueServerService;
            if (queueServerService != null)
            {
                queueServerService.SendData(e.Sender, Encoding.UTF8.GetBytes(e.Message));
            }
        }

        public void Stop()
        {
        }
    }
}
