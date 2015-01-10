using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.BusinessLayer.Models;

namespace Communicator.BusinessLayer.Interfaces
{
    public interface ILogicClient
    {
        event RepeaterEventHandler Repeater;
        void RegisterUser(UserModel user);
    }
}
