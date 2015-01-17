using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.BusinessLayer.Enums;
using Communicator.BusinessLayer.Interfaces;
using Communicator.Protocol.Notifications;
using Communicator.Protocol.Requests;
using Communicator.Protocol.Responses;
using Communicator.Queue;
using Communicator.Untils.Serializers;

namespace Communicator.BusinessLayer.Services
{
    public class MessageRecoginzerClientService : IMessageRecognizerClientService
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

            if (type == typeof(CreateUserResponse))
            {
                CreateUserProcess(message);
                return;
            }

            if (type == typeof(AuthResponse))
            {
                LoginUserProcess(message);
                return;
            }

            if (type == typeof(UserListResponse))
            {
                UserListProcess(message);
                return;
            }

            if (type == typeof(MessageReq))
            {
                MessageProcess(message);
                return;
            }

            if (type == typeof(PresenceStatusNotification))
            {
                PresenceNotificationProgress(message);
                return;
            }

            if (type == typeof (ActivityNotification))
            {
                ActivityNotificationProcess(message);
                return;
            }



        }

        private void ActivityNotificationProcess(MessageReceivedEventArgs message)
        {
            var activityNotification = _serializerService.Deserialize<ActivityNotification>(message.Message);

            OnRepeater(ActionTypes.UserWriting, true, activityNotification);
        }

        private void PresenceNotificationProgress(MessageReceivedEventArgs message)
        {
            var presenceNotification = _serializerService.Deserialize<PresenceStatusNotification>(message.Message);

            OnRepeater(ActionTypes.PresenceNotification, true, presenceNotification);
        }

        private void MessageProcess(MessageReceivedEventArgs message)
        {
            var messageReq = _serializerService.Deserialize<MessageReq>(message.Message);

            OnRepeater(ActionTypes.Message, true, messageReq);
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
                Repeater(this, new RepeaterEventArgs { Type = actionTypes, Result = result, Data = data });
            }
        }
    }
}
