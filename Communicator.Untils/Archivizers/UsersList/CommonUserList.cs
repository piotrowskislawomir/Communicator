using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Communicator.Protocol.Requests;

namespace Communicator.Untils.Archivizers.UsersList
{
    public class CommonUserList
    {
        private static List<User> commonList = null;
        private static string pathToFile = "";
       
        //przy starcie serwera wczytanie użytkowników gdy plik istnieje gdy nie to pusta lista
        public static void LoadAllUsersFromFile(string path)
        {
            pathToFile = path;
            if(File.Exists(path))
            {
               XDocument doc = XDocument.Load(path);
               commonList = (from c in doc.Descendants("User")
                         select new User
                         {
                             Login = (string)c.Attribute("Login"),
                         }).ToList();
           }
           else commonList = new List<User>();
        }

        // sprawdzanie dostępności loginu
        public static bool CreateNewUser(CreateUserReq user)
        {
            bool createSuccesfully = true;
           
            foreach (var us in commonList)
            {
                if (us.Login == user.Login)
                {
                    createSuccesfully = false;
                    var newUser = new User();
                    us.Login = user.Login;
                    us.Password = user.Password;
                    commonList.Add(newUser);
                    XmlUserList.CreateNewUser(pathToFile, user);
                }
            }

            return createSuccesfully; // true taki użytkownik istnieje false nie istnieje i został stworzony
        }

        public static bool UserAuthentication(AuthRequest user)
        {
            bool exist = false;
           
            foreach (var us in commonList)
            {
                if (us.Login == user.Login && us.Password == user.Password)
                    exist = true;
            }
           
            return exist; 
        }

    }
}
