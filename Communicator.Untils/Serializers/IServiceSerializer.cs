
namespace Communicator.Untils.Serializers
{
    public interface IServiceSerializer
    {
            byte[] Serialize<T>(T dto);
            T Deserialize<T>(byte[] data);
    }
}
