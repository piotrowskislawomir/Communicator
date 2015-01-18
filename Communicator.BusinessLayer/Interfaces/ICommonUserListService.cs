using System.Collections.Generic;
using Communicator.BusinessLayer.Models;
using Communicator.Protocol.Requests;

namespace Communicator.BusinessLayer.Interfaces
{
    public interface ICommonUserListService
    {
        string FilePath { get; set; }
        void LoadAllUsersFromFile();

        // sprawdzanie dostępności loginu
        bool CreateNewUser(CreateUserReq user);

        bool UserAuthentication(AuthRequest user);

        bool UserExist(MessageReq mr);

        List<CommonUsers> GetUsers();
    }
}