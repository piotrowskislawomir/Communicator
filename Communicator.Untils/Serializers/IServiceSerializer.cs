
namespace Communicator.Untils.Serializers
{
    interface IServiceSerializer
    {
            byte[] Serialize<T>(T dto);
            T Deserialize<T>(byte[] data);
    }
}
