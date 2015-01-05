using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Protocol.Model;
using Communicator.Protocol.Requests;
using Communicator.Protocol.Responses;
using Communicator.Untils;
using Communicator.Untils.Serializers;
using Communicator.Untils.Archivizers.UsersList;

namespace Communicator.Untils
{
    class Simula
    {
        public static void Simulation()
        {
            #region SYMULACJA REJESTRACJI AUTENTYKACJI I POBIERANIA LISTY UŻYTKOWNIKÓW
            CommonUserList.LoadAllUsersFromFile("C:\\lista.xml");

            var us1 = new CreateUserReq();
            us1.Login = "s34353";
            us1.Password = "tajehasło";

            var us2 = new CreateUserReq();
            us2.Login = "s34353";
            us2.Password = "tadshasło";

            var us3 = new CreateUserReq();
            us3.Login = "s34358";
            us3.Password = "tajehasło";


            CommonUserList.CreateNewUser(us1);
            CommonUserList.CreateNewUser(us2);
            CommonUserList.CreateNewUser(us3);


            var ar1 = new AuthRequest();
            ar1.Login = "s34353";
            ar1.Password = "tajehasło";

            var ar2 = new AuthRequest();
            ar2.Login = "Karol";
            ar2.Password = "ŹÓŁŻĄ";

            var ar3 = new AuthRequest();
            ar3.Login = "s34358";
            ar3.Password = "tajehasło";


            Console.WriteLine(CommonUserList.UserAuthentication(ar1));
            Console.WriteLine(CommonUserList.UserAuthentication(ar2));
            Console.WriteLine(CommonUserList.UserAuthentication(ar3));

            var ulr = new UserListReq();
            ulr.Login = "s34353";

            List<User> list = ActivityUserList.GetList(ulr);

            foreach (var user in list)
            {
                Console.WriteLine(user.Login + " " + user.Status);
            }

            # endregion

 }   
    }
}
