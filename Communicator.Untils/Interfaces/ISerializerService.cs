namespace Communicator.Untils.Interfaces
{
    public interface ISerializerService
    {
        byte[] Serialize<T>(T dto);
        T Deserialize<T>(byte[] data);
    }
}