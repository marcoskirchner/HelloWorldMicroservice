using System;
using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using HelloWorldMicroservice.Domain;

namespace HelloWorldMicroservice.Messaging
{
    public class KafkaHelloWorldSerializer : ISerializer<HelloWorldMessage>
    {
        public byte[] Serialize(HelloWorldMessage data, SerializationContext context)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
        }
    }

    public class KafkaHelloWorldDeserializer : IDeserializer<HelloWorldMessage>
    {
        public HelloWorldMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return null;
            }
            return JsonSerializer.Deserialize<HelloWorldMessage>(Encoding.UTF8.GetString(data));
        }
    }
}
