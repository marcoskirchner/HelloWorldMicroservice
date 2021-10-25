using System.Text;
using System.Text.Json;

namespace HelloWorldMicroservice.Messaging
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Methods are used on object instances")]
    public class MessageSerializer
    {
        public byte[] Serialize<T>(T value)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
        }

        public T Deserialize<T>(byte[] data)
        {
            return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(data));
        }
    }
}
