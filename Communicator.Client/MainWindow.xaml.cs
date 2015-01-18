using System;
using Autofac;
using Communicator.BusinessLayer.Interfaces;
using Communicator.Client.ViewModels;

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
            loginViewModel.OnRequestClose += (s, e) => Close();
            DataContext = loginViewModel;
        }
    }
}