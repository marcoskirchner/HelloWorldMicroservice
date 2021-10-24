using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using HelloWorldMicroservice.Configs;
using HelloWorldMicroservice.Domain;
using Microsoft.Extensions.Options;

namespace HelloWorldMicroservice.Messaging
{
    public class KafkaHelloWorldReceiver : IHelloWorldReceiver, IDisposable
    {
        private readonly IConsumer<Null, HelloWorldMessage> _consumer;

        public KafkaHelloWorldReceiver(IOptions<KafkaConfig> kafkaconfig)
        {
            var conf = new ConsumerConfig()
            {
                BootstrapServers = kafkaconfig.Value.Servers,
                GroupId = Guid.NewGuid().ToString(),
            };
            _consumer = new ConsumerBuilder<Null, HelloWorldMessage>(conf)
                .SetValueDeserializer(new KafkaHelloWorldDeserializer())
                .Build();
            _consumer.Subscribe(kafkaconfig.Value.Topic);
        }

        public async Task<HelloWorldMessage> ReceiveHelloWorldAsync(CancellationToken stoppingToken)
        {
            var kafkaMsg = _consumer.Consume(0);
            if (kafkaMsg == null)
            {
                kafkaMsg = await Task.Run(() => _consumer.Consume(stoppingToken), stoppingToken);
            }

            return kafkaMsg.Message.Value;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _consumer.Unsubscribe();
            _consumer.Close();
            _consumer.Dispose();
        }
    }
}
