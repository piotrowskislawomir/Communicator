using System.Configuration;
using System.Linq;
using Communicator.Untils.Interfaces;

namespace Communicator.Untils.Services
{
    public class XmlConfigurationService : IConfigurationService
    {
        public string Host
        {
            get { return GetValueFromConfig("host", "winserver2012"); }
        }
        public string UserName
        {
            get { return GetValueFromConfig("username", "guest"); }
        }
        public string Password
        {
            get { return GetValueFromConfig("password", "guest"); }
        }
        public string ExchangeName
        {
            get { return GetValueFromConfig("exchangename", "CommunicatorExchange"); }
        }

        public string MainQueueName
        {
            get { return GetValueFromConfig("mainqueuename", "CommunicatorMainQueue"); }
        }

        public string UserListFileName
        {
            get { return GetValueFromConfig("userlistfilename", "C:\\userList.xml"); }
        }

        public string ArchiveFileName
        {
            get { return GetValueFromConfig("archivefilename", "C:\\archive.xml"); }
        }

        private static string GetValueFromConfig(string key, string defaultValue)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return ConfigurationManager.AppSettings[key];
            }

            return defaultValue;
        }
    }
}
