using Communicator.BusinessLayer.Interfaces;
using Communicator.Queue;
using Communicator.Queue.Interfaces;
using Communicator.Untils.Configuration;

namespace Communicator.Server
{
    public class ServerApplication
    {
        private readonly IQueueServerService _queueServerService;
        private readonly IConfigurationService _configurationService;
        private readonly IMessageRecognizerService _messageRecognizerService;

        public ServerApplication(IQueueServerService queueServerService,
            IConfigurationService configurationService,
            IMessageRecognizerService messageRecognizerService )
        {
            _queueServerService = queueServerService;
            _configurationService = configurationService;
            _messageRecognizerService = messageRecognizerService;

            _messageRecognizerService.QueueServerService = queueServerService;
            _messageRecognizerService.ConfigurationService = configurationService;
        }

        public void Start()
        {
            CreateNewListener();
        }

        private void CreateNewListener()
        {
            _queueServerService.Initialize(_configurationService.Host, _configurationService.UserName,
                _configurationService.Password, _configurationService.ExchangeName);
            _queueServerService.MessageReceived += MessageReceived;
            _queueServerService.CreateConsumer(_configurationService.MainQueueName, _configurationService.ExchangeName);
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            _messageRecognizerService.ProcessMessage(e);
        }

        public void Stop()
        {
        }
    }
}
