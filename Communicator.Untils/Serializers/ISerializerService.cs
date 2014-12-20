
namespace Communicator.Untils.Serializers
{
    public interface ISerializerService
    {
            byte[] Serialize<T>(T dto);
            T Deserialize<T>(byte[] data);
    }
}
