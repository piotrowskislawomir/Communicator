using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.BusinessLayer.Models;
using Communicator.Protocol.Enums;

namespace Communicator.BusinessLayer.Interfaces
{
    public interface ILogicClient
    {
        event RepeaterEventHandler Repeater;
        string Login { get; set; }
        string RouteKey { get; set; }
        void RegisterUser(UserModel user);
        void LoginUser(UserModel user);
        void GetUserList();
        void SendMessage(string recipient, string message, byte[] imageData);
        void SendPing(PresenceStatus status);
        void SendUserWriting(string recipient);
        void Initialize();
    }


}
