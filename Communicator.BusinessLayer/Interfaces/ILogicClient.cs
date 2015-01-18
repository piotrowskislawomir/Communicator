using Communicator.BusinessLayer.Models;
using Communicator.Protocol.Enums;

namespace Communicator.BusinessLayer.Interfaces
{
    public interface ILogicClient
    {
        string Login { get; set; }
        string RouteKey { get; set; }
        event RepeaterEventHandler Repeater;
        void RegisterUser(UserModel user);
        void LoginUser(UserModel user);
        void GetUserList();
        void SendMessage(string recipient, string message, byte[] imageData);
        void SendPing(PresenceStatus status);
        void SendUserWriting(string recipient);
        void Initialize();
        void GetHistory();
    }
}