using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Communicator.BusinessLayer.Interfaces;
using Communicator.BusinessLayer.Models;
using Communicator.Protocol.Model;
using Communicator.Protocol.Requests;

namespace Communicator.BusinessLayer.Services
{
    public class CommonUserListService : ICommonUserListService
    {
        List<CommonUsers> _commonList = new List<CommonUsers>();
        public string FilePath { get; set; }

        //przy starcie serwera wczytanie użytkowników gdy plik istnieje gdy nie to pusta lista
        public void LoadAllUsersFromFile()
        {
            if (File.Exists(FilePath))
            {
                XDocument doc = XDocument.Load(FilePath);
                _commonList = (from c in doc.Descendants("User")
                               select new CommonUsers
                               {
                                   Login = (string)c.Attribute("Login"),
                                   Password = (string)c.Attribute("Password")
                               }).ToList();
            }
            else _commonList = new List<CommonUsers>();
        }

        // sprawdzanie dostępności loginu

        public bool CreateNewUser(CreateUserReq user)
        {
            bool createSuccesfully = true;

            foreach (var us in _commonList)
            {
                if (us.Login == user.Login)
                    createSuccesfully = false;
            }

            if (createSuccesfully)
            {
                var newUser = new CommonUsers();
                newUser.Login = user.Login;
                newUser.Password = user.Password;
                XmlUserList.CreateNewUser(FilePath, user);
                _commonList.Add(newUser);
            }

            return createSuccesfully; // true taki użytkownik istnieje false nie istnieje i został stworzony
        }

        public bool UserAuthentication(AuthRequest user)
        {
            foreach (var us in _commonList)
            {
                if (us.Login == user.Login && us.Password == user.Password)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UserExist(MessageReq mr)
        {
            foreach (var us in _commonList)
            {
                if (us.Login == mr.Recipient)
                {
                    return true;
                }
            }
            return false;
        }

        public List<CommonUsers> GetUsers()
        {
            return _commonList;
        }

    }

}
