using System.Reflection;
using Autofac;
using Communicator.BusinessLayer.Interfaces;
using Communicator.BusinessLayer.Services;
using Communicator.Queue.Interfaces;
using Communicator.Queue.Services;
using Communicator.Untils.Archivizers.Message;
using Communicator.Untils.Interfaces;
using Communicator.Untils.Services;

namespace Communicator.Server
{
    public static class InstanceContainer
    {
        public static IContainer Container;

        public static void Init()
        {
            var builder = new ContainerBuilder();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(executingAssembly)
                .AsSelf()
                .AsImplementedInterfaces();

            // override default registration if needed
            builder.RegisterType<ServerApplication>().As<IServerApplication>();
            builder.RegisterType<JSonSerializerService>().As<ISerializerService>();
            builder.RegisterType<XmlConfigurationService>().As<IConfigurationService>();
            builder.RegisterType<RabbitMqConnection>().As<IQueueConnection>();
            builder.RegisterType<RabbitMqClientService>().As<IQueueClientService>();
            builder.RegisterType<RabbitMqServerService>().As<IQueueServerService>();
            builder.RegisterType<RabbitMqQueueManagerService>().As<IQueueManagerService>();
            builder.RegisterType<MessageRecognizerService>().As<IMessageRecognizerService>();
            builder.RegisterType<MessageRecoginzerClientService>().As<IMessageRecognizerClientService>();
            builder.RegisterType<CommonUserListService>().As<ICommonUserListService>();
            builder.RegisterType<LogicClient>().As<ILogicClient>();
            builder.RegisterType<XmlMessageArchivizer>().As<IMessageArchivizer>();

            Container = builder.Build();
        }
    }
}