using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.BusinessLayer.Enums;
using Communicator.BusinessLayer.Interfaces;
using Communicator.Protocol.Responses;
using Communicator.Queue;
using Communicator.Untils.Serializers;

namespace Communicator.BusinessLayer.Services
{
    public class MessageRecoginzerClientService:IMessageRecognizerClientService
    {
        private readonly ISerializerService _serializerService;

        public MessageRecoginzerClientService(ISerializerService serializerService)
        {
            _serializerService = serializerService;
        }

        public event RepeaterEventHandler Repeater;

        public void ProceedMessage(MessageReceivedEventArgs message)
        {
            var type = Type.GetType(message.ContentType);

            if (type == typeof (CreateUserResponse))
            {
                CreateUserProcess(message);
                return;
            }

            if (type == typeof (AuthResponse))
            {
                LoginUserProcess(message);
            }

            if (type == typeof (UserListResponse))
            {
                UserListProcess(message);
            }
        }

        private void UserListProcess(MessageReceivedEventArgs message)
        {
            var userListResponse = _serializerService.Deserialize<UserListResponse>(message.Message);

            OnRepeater(ActionTypes.ContactList, true, userListResponse.Users);
        }

        private void LoginUserProcess(MessageReceivedEventArgs message)
        {
            var authResponse = _serializerService.Deserialize<AuthResponse>(message.Message);

            OnRepeater(ActionTypes.Login, authResponse.IsAuthenticated);
        }

        private void CreateUserProcess(MessageReceivedEventArgs message)
        {
            var createUserResponse = _serializerService.Deserialize<CreateUserResponse>(message.Message);

            OnRepeater(ActionTypes.UserCreate, createUserResponse.CreatedSuccessfully);
        }

        public void OnRepeater(ActionTypes actionTypes, bool result, object data = null)
        {
            if (Repeater != null)
            {
                Repeater(this, new RepeaterEventArgs{Type = actionTypes, Result=result, Data = data});
            }
        }
    }
}
