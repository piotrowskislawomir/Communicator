using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator.Untils
{
    public static class ConfigurationApp
    {
        public static string Host {
            get { return "winserver2012"; }
            set { }
        }
        public static string UserName
        {
            get { return "guest"; }
            set { }
        }
        public static string Password
        {
            get { return "guest"; }
            set { }
        }
        public static string ExchangeName
        {
            get { return "CommunicatorExchange"; }
            set { }
        }
    }
}
