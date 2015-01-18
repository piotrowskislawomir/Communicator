using Communicator.BusinessLayer.Interfaces;
using Communicator.Queue;
using Communicator.Queue.Interfaces;
using Communicator.Untils.Interfaces;

namespace Communicator.Server
{
    public class ServerApplication : IServerApplication
    {
        private readonly IConfigurationService _configurationService;
        private readonly IMessageRecognizerService _messageRecognizerService;
        private readonly IQueueServerService _queueServerService;

        public ServerApplication(IQueueServerService queueServerService,
            IConfigurationService configurationService,
            IMessageRecognizerService messageRecognizerService)
        {
            _queueServerService = queueServerService;
            _configurationService = configurationService;
            _messageRecognizerService = messageRecognizerService;

            _messageRecognizerService.QueueServerService = queueServerService;
            _messageRecognizerService.ConfigurationService = configurationService;
            _messageRecognizerService.Initialize();
        }

        public void Start()
        {
            CreateNewListener();
        }

        public void Stop()
        {
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
    }
}