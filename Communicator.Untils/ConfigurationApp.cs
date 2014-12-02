using System.Configuration;
using System.Linq;

namespace Communicator.Untils
{
    public static class ConfigurationApp
    {
        public static string Host {
            get { return GetValueFromConfig("host","winserver2012"); }
        }
        public static string UserName
        {
            get { return GetValueFromConfig("username","guest"); }
        }
        public static string Password
        {
            get { return GetValueFromConfig("password","guest"); }
        }
        public static string ExchangeName
        {
            get { return GetValueFromConfig("exchangename", "CommunicatorExchange"); }
        }

        public static string MainQueueName
        {
            get { return GetValueFromConfig("mainqueuename","CommunicatorMainQueue"); }
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
