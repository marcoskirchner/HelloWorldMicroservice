using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using HelloWorldMicroservice.Configs;
using HelloWorldMicroservice.Domain;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HelloWorldMicroservice.Messaging
{
    public class RabbitMqHelloWorldReceiver : IHelloWorldReceiver, IDisposable
    {
        private readonly RabbitMqConfig _rabbitMqConfig;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly MessageSerializer _serializer;

        public RabbitMqHelloWorldReceiver(IOptions<RabbitMqConfig> rabbitMqConfig, MessageSerializer serializer)
        {
            _rabbitMqConfig = rabbitMqConfig.Value;
            _serializer = serializer;

            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqConfig.HostName,
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_rabbitMqConfig.ExchangeName, ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, _rabbitMqConfig.ExchangeName, string.Empty);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;

            _channel.BasicConsume(queueName, false, consumer);
        }

        private readonly BlockingCollection<HelloWorldMessage> _queue = new();

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = _serializer.Deserialize<HelloWorldMessage>(e.Body.ToArray());
            _queue.Add(message);
            _channel.BasicAck(e.DeliveryTag, false);
        }

        public Task<HelloWorldMessage> ReceiveHelloWorldAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => _queue.Take(), stoppingToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _channel.Close();
            _channel.Dispose();
            _connection.Close();
            _connection.Dispose();
        }

    }
}
