using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using HelloWorldMicroservice.Configs;
using HelloWorldMicroservice.Domain;
using Microsoft.Extensions.Options;

namespace HelloWorldMicroservice.Messaging
{
    public class KafkaHelloWorldSender : IHelloWorldSender, IDisposable
    {
        private readonly KafkaConfig _kafkaConfig;
        private readonly IProducer<Null, HelloWorldMessage> _producer;

        public KafkaHelloWorldSender(IOptions<KafkaConfig> kafkaConfig)
        {
            _kafkaConfig = kafkaConfig.Value;

            var conf = new ProducerConfig()
            {
                BootstrapServers = _kafkaConfig.Servers,
            };
            _producer = new ProducerBuilder<Null, HelloWorldMessage>(conf)
                .SetValueSerializer(new KafkaHelloWorldSerializer())
                .Build();
        }

        public async Task SendHelloWorldAsync(HelloWorldMessage message, CancellationToken stoppingToken)
        {
            var kafkaMsg = new Message<Null, HelloWorldMessage>()
            {
                Value = message,
            };
            await _producer.ProduceAsync(_kafkaConfig.Topic, kafkaMsg, stoppingToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _producer.Dispose();
        }
    }
}
