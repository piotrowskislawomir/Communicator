using System.IO;
using System.Runtime.Serialization.Json;
using Communicator.Untils.Interfaces;

namespace Communicator.Untils.Services
{
    public class JSonSerializerService : ISerializerService
    {
        public byte[] Serialize<T>(T dto)
        {
            using (var stream = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(stream, dto);
                return stream.ToArray();
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                T dt = (T)ser.ReadObject(stream);
                return dt;
            }
        }
    }
}
