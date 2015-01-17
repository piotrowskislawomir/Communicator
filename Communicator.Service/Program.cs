using Autofac;
using Communicator.BusinessLayer.Services;
using Communicator.Queue.Services;
using Communicator.Untils.Services;
using Topshelf;

namespace Communicator.Server
{
    class Program
    {
        static void Main(string[] args)
        {

            var host = HostFactory.New(x =>
            {
                InstanceContainer.Init();
                x.Service<ServerApplication>(s =>
                {
                    //s.ConstructUsing(name => new ServerApplication(new RabbitMqServerService(new RabbitMqConnection(), new JSonSerializerService()), new XmlConfigurationService(), new MessageRecognizerService(new RabbitMqQueueManagerService(new RabbitMqConnection()),new JSonSerializerService(), new CommonUserListService() )) );
                    s.ConstructUsing(name => (ServerApplication) InstanceContainer.Container.Resolve<IServerApplication>());
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
