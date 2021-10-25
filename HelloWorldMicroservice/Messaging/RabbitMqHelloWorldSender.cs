using System;
using System.Threading;
using System.Threading.Tasks;
using HelloWorldMicroservice.Configs;
using HelloWorldMicroservice.Domain;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace HelloWorldMicroservice.Messaging
{
    public class RabbitMqHelloWorldSender : IHelloWorldSender, IDisposable
    {
        private readonly RabbitMqConfig _rabbitMqConfig;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly MessageSerializer _serializer;

        public RabbitMqHelloWorldSender(IOptions<RabbitMqConfig> rabbitMqConfig, MessageSerializer serializer)
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
        }

        public Task SendHelloWorldAsync(HelloWorldMessage message, CancellationToken stoppingToken)
        {
            return Task.Run(() => _channel.BasicPublish(
                _rabbitMqConfig.ExchangeName,
                string.Empty,
                null,
                _serializer.Serialize(message)), stoppingToken);
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
