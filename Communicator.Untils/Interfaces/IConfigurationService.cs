namespace Communicator.Untils.Interfaces
{
    public interface IConfigurationService
    {
        string Host { get; }
        string UserName { get; }
        string Password { get; }
        string ExchangeName { get; }
        string MainQueueName { get; }
        string UserListFileName { get; }
        string ArchiveFileName { get; }
    }
}