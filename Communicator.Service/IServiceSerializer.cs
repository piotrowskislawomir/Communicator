
namespace Communicator.Service
{
    interface IServiceSerializer
    {
            byte[] Serialize<T>(T dto);
            T Deserialize<T>(byte[] data);
    }
}
