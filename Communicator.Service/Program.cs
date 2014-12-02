using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Queue.Services;
using Communicator.Untils;

namespace Communicator.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new RabbitMqServerService(new RabbitMqConnection());
            s.Initialize();
            s.CreateConsumer(ConfigurationApp.MainQueueName);


            var q = new RabbitMqClientService(new RabbitMqConnection());
            q.Initialize();
            string key = q.GetUniqueTopic("login");
            q.MessageReceived += (_, e) => Console.WriteLine(e.Message);
            q.CreateConsumer(key);

            var q1 = new RabbitMqClientService(new RabbitMqConnection());
            q1.Initialize();
            string key1 = q1.GetUniqueTopic("login");
            q1.MessageReceived += (_, e) => Console.WriteLine(e.Message);
            q1.CreateConsumer(key1);

            q.SendData(ConfigurationApp.MainQueueName, key, Encoding.UTF8.GetBytes("dupa"));
            Console.ReadKey();
        }
    }
}
