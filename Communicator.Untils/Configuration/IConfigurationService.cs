namespace Communicator.Untils.Configuration
{
    public interface IConfigurationService
    {
        string Host { get; }
        string UserName { get; }
        string Password { get; }
        string ExchangeName { get; }
        string MainQueueName { get; }
        string UserListFileName { get; }
    }
}
