using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Protocol.Requests;

namespace Communicator.Untils.Archivizers.UsersList
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
