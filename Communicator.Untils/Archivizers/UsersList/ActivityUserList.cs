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
             return ActivityList.Any(user => user.Login == mReq.Recipient);
        }
        
        public static List<User> GetList(UserListReq user)
        {
            var userListResponse = new List<User>();
            if(ActivityList != null)
            {
                userListResponse.AddRange(ActivityList.Where(u => u.Login != user.Login));
            }
            return userListResponse;
        }

        public static void AddToList(AuthRequest user)
        {
            if (ActivityList == null)  ActivityList = new List<User>();
            var newActiveUser = new User {Login = user.Login, Status = PresenceStatus.Online};
            ActivityList.Add(newActiveUser);
        }

        public void ChangeStatus(User user, PresenceStatus newStatus)
        {
            foreach (var us in ActivityList.Where(us => us.Login == user.Login))
            {
                us.Status = newStatus;
                return;
            }
        }

        public void DeleteFromList(User user)
        {
            var activeUser = ActivityList.SingleOrDefault(u => u.Login == user.Login);
            if(activeUser != null)
            {
                ActivityList.Remove(activeUser);
            }
        }
    }
}
