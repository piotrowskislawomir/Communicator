using System.Configuration;
using System.Linq;

namespace Communicator.Untils.Configuration
{
    public class XmlConfigurationService:IConfigurationService
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
