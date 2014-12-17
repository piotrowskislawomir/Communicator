using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Protocol.Requests;

namespace Communicator.Untils.Archivizers.UsersList
{
    interface IUsersListOperationer
    {
        bool CreateNewUser(CreateUserReq userReg, string path);
        bool AuthenticationUser(AuthRequest userAuth, string path);

        // user żeby nie zczytać własnego loginu
        List<User> ReadList(User user, string path); 
    }
}
