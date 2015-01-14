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
<<<<<<< HEAD
			// Ten blok należy zakomentować...
			
			CommunicatorWindow cm = new CommunicatorWindow();
	        cm.Show();

			ConversationWindow cm1 = new ConversationWindow();
	        cm1.Show();

			// ...Dotąd

			InitializeComponent();
			// Ten należy odkomentować

            //var clientLogic = new LogicClient(new RabbitMqClientService(new RabbitMqConnection(), new JSonSerializerService()), new XmlConfigurationService(), new MessageRecoginzerClientService(new JSonSerializerService()));
            //clientLogic.RouteKey = Guid.NewGuid().ToString();
            //clientLogic.Initialize();
            //DataContext = new LoginViewModel(clientLogic);
=======
            InitializeComponent();
            var clientLogic = new LogicClient(new RabbitMqClientService(new RabbitMqConnection(), new JSonSerializerService()), new XmlConfigurationService(), new MessageRecoginzerClientService(new JSonSerializerService()));
            clientLogic.RouteKey = Guid.NewGuid().ToString();
            clientLogic.Initialize();
            var loginViewModel = new LoginViewModel(clientLogic);
            loginViewModel.OnRequestClose += (s, e) => this.Close();
            DataContext = loginViewModel;
>>>>>>> cff151ec7034986c4cf008861bbcd5a941a78b98
            // SetDictionary();
        }
       /* private void SetDictionary()
        {
            var dict = new ResourceDictionary();
            dict.Source = new Uri("..\\Resources\\NamesDictionary.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dict);
            this.Resources.MergedDictionaries.Add(dict);
        }*/

    }
}
