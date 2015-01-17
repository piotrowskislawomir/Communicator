using System;
using Autofac;
using Communicator.BusinessLayer.Interfaces;
using Communicator.BusinessLayer.Services;
using Communicator.Client.ViewModels;
using Communicator.Queue.Services;
using Communicator.Untils.Services;

namespace Communicator.Client
{
    public sealed partial class MainWindow : ApplicationWindowBase
    {       
        public MainWindow()
        {
            InstanceContainer.Init();
            var clientLogic = InstanceContainer.Container.Resolve<ILogicClient>();
            clientLogic.RouteKey = Guid.NewGuid().ToString();
            clientLogic.Initialize();
            var loginViewModel = new LoginViewModel(clientLogic);
            loginViewModel.OnRequestClose += (s, e) => this.Close();
            DataContext = loginViewModel;
        }

    }
}
