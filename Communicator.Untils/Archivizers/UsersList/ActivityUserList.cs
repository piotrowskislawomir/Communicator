using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Protocol.Enums;
using Communicator.Protocol.Model;
using Communicator.Protocol.Requests;

namespace Communicator.Untils.Archivizers.UsersList
{
    public class ActivityUserList
    {
        private static List<User> ActivityList = null;


        public static bool CheckUserAvalibility(MessageReq mReq)
        {
            bool avaliable = false;
            foreach (var user in ActivityList)
            {
                if (user.Login == mReq.Recipient)
                    avaliable = true;
            }
            return avaliable;

        }
        
        public static List<User> GetList(UserListReq user)
        {
            var userListResponse = new List<User>();
            foreach (var u in ActivityList)
            {
                if (u.Login != user.Login)
                    userListResponse.Add(u);
            }
            return userListResponse;
        }

        public static void AddToList(AuthRequest user)
        {
            var newActiveUser = new User();
            newActiveUser.Login = user.Login;
            newActiveUser.Status = PresenceStatus.Online;
        }

        public void ChangeStatus(User user, PresenceStatus newStatus)
        {
            foreach (var us in ActivityList)
            {
                if (us.Login == user.Login)
                {
                    us.Status = newStatus;
                }
            }
           
        }

        public void DeleteFromList(User user)
        {
            int count = 0;
            foreach (var us in ActivityList)
            {
                if (us.Login == user.Login)
                    ActivityList.RemoveAt(count);
                
                count++;
            }
            
        }
    }
}
