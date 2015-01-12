using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.BusinessLayer.Interfaces;
using Communicator.BusinessLayer.Models;
using Communicator.Protocol.Requests;
using Communicator.Queue.Interfaces;
using Communicator.Untils.Archivizers.UsersList;
using Communicator.Untils.Configuration;

namespace Communicator.BusinessLayer.Services
{
    public class LogicClient:ILogicClient
    {
        private readonly IQueueClientService _queueClientService;
        private readonly IConfigurationService _configurationService;
        private readonly IMessageRecognizerClientService _messageRecognizerClientService;

        public string RouteKey { get; set; }
        public string Login { get; set; }

        public LogicClient(IQueueClientService queueClientService, IConfigurationService configurationService, IMessageRecognizerClientService messageRecognizerClientService)
        {
            _queueClientService = queueClientService;
            _configurationService = configurationService;
            _messageRecognizerClientService = messageRecognizerClientService;
        }

        public void Initialize()
        {
            _queueClientService.Initialize(_configurationService.Host, _configurationService.UserName, _configurationService.Password, _configurationService.ExchangeName);
            _queueClientService.MessageReceived += (_, ee) => _messageRecognizerClientService.ProceedMessage(ee);
            _queueClientService.CreateConsumer(RouteKey,_configurationService.ExchangeName);
            _messageRecognizerClientService.Repeater += OnRepeater;
        }

        public event RepeaterEventHandler Repeater;

        public void OnRepeater(object sender, RepeaterEventArgs e)
        {
            if (Repeater != null)
            {
                Repeater(this, e);
            }
        }

        public void RegisterUser(UserModel user)
        {
            var createUserReq = new CreateUserReq {Login = user.Login, Password = user.Password};
            _queueClientService.SendData(_configurationService.MainQueueName, RouteKey, _configurationService.ExchangeName, createUserReq);
        }

        public void LoginUser(UserModel user)
        {
            var authReq = new AuthRequest {Login = user.Login, Password = user.Password};
            _queueClientService.SendData(_configurationService.MainQueueName, RouteKey, _configurationService.ExchangeName, authReq);

        }

        public void GetUserList()
        {
            var userListReq = new UserListReq() {Login = Login};
            _queueClientService.SendData(_configurationService.MainQueueName, RouteKey, _configurationService.ExchangeName, userListReq);
        }
    }
}
