using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Protocol.Enums;
using Communicator.Protocol.Requests;

namespace Communicator.Untils.Archivizers.UsersList
{
    class ActivityUserList
    {
        private static List<User> ActivityList = null;


        public void CheckUserAvalibility()
        {
         
            
        }
        
            public void GetList()
        {
            
        }

        public void AddToList(AuthRequest user)
        {
            var newActiveUser = new User();
            newActiveUser.Login = user.Login;
           
           // newActiveUser.
        }

        public void ChangeStatus()
        {
            
        }

        public void DeleteFromList()
        {
            
        }
    }
}
