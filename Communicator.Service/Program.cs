using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Queue.Services;
using Communicator.Untils;
using Topshelf;

namespace Communicator.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = HostFactory.New(x =>
            {
                x.Service<ServerApplication>(s =>
                {
                    s.ConstructUsing(name => new ServerApplication());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("SampleService Description");
                x.SetDisplayName("SampleService");
                x.SetServiceName("SampleService");
            });

            host.Run();
        }
    }
}
