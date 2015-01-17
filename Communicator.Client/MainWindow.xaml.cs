using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Communicator.BusinessLayer.Services;
using Communicator.Client.ViewModels;
using Communicator.Queue.Services;
using Communicator.Untils.Configuration;
using Communicator.Untils.Serializers;

namespace Communicator.Client
{
    public sealed partial class MainWindow : ApplicationWindowBase
    {       
        public MainWindow()
        {
            var clientLogic = new LogicClient(new RabbitMqClientService(new RabbitMqConnection(), new JSonSerializerService()), new XmlConfigurationService(), new MessageRecoginzerClientService(new JSonSerializerService()));
            clientLogic.RouteKey = Guid.NewGuid().ToString();
            clientLogic.Initialize();
            var loginViewModel = new LoginViewModel(clientLogic);
            loginViewModel.OnRequestClose += (s, e) => this.Close();
            DataContext = loginViewModel;
        }

    }
}
