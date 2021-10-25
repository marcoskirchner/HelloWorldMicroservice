using System;
using Confluent.Kafka;
using HelloWorldMicroservice.Domain;

namespace HelloWorldMicroservice.Messaging
{
    public class KafkaHelloWorldSerializer : MessageSerializer, ISerializer<HelloWorldMessage>
    {
        byte[] ISerializer<HelloWorldMessage>.Serialize(HelloWorldMessage data, SerializationContext context)
        {
            return Serialize(data);
        }
    }

    public class KafkaHelloWorldDeserializer : MessageSerializer, IDeserializer<HelloWorldMessage>
    {
        HelloWorldMessage IDeserializer<HelloWorldMessage>.Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return null;
            }
            return Deserialize<HelloWorldMessage>(data.ToArray());
        }
    }
}
