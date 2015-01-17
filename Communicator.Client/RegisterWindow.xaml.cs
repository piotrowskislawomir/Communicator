using System;
using Communicator.BusinessLayer.Services;
using Communicator.Client.ViewModels;
using Communicator.Queue.Services;
using Communicator.Untils.Services;

namespace Communicator.Client
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public sealed partial class RegisterWindow : ApplicationWindowBase
    {
        public RegisterWindow()
        {
            InitializeComponent();
            var clientLogic = new LogicClient(new RabbitMqClientService(new RabbitMqConnection(), new JSonSerializerService()),new XmlConfigurationService(), new MessageRecoginzerClientService(new JSonSerializerService()));
            clientLogic.RouteKey = Guid.NewGuid().ToString();
            clientLogic.Initialize();
            DataContext = new RegisterViewModel(clientLogic);
        }

    }
}
