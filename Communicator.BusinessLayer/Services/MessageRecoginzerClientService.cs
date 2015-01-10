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
            }
        }

        private void CreateUserProcess(MessageReceivedEventArgs message)
        {
            var createUserResponse = _serializerService.Deserialize<CreateUserResponse>(message.Message);

            if (createUserResponse.CreatedSuccessfully)
            {
                OnRepeater(ActionTypes.UserCreatedSuccess);
            }
          }

        public void OnRepeater(ActionTypes actionTypes)
        {
            if (Repeater != null)
            {
                Repeater(this, new RepeaterEventArgs{Type = actionTypes});
            }
        }
    }
}
