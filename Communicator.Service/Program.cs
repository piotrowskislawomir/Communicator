using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Queue.Services;

namespace Communicator.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var q = new RabbitMqService(new RabbitMqConnection());
            q.Initialize();
            string key = q.GetUniqueTopic();
            q.MessageReceived += (_, e) => Console.WriteLine(e.Message);
            q.CreateConsumer(key);

            q.SendData(key, Encoding.UTF8.GetBytes("msg"));

            Console.ReadKey();
        }
    }
}
