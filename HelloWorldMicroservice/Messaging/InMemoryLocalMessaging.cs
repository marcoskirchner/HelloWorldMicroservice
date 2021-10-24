using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using HelloWorldMicroservice.Domain;

namespace HelloWorldMicroservice.Messaging
{
    /// <summary>
    /// Uses an in-memory queue to hold messages.
    /// For testing-only, does not work with multiple instances.
    /// </summary>
    public class InMemoryLocalMessaging : IHelloWorldReceiver, IHelloWorldSender
    {
        private readonly BlockingCollection<HelloWorldMessage> _queue = new();

        public Task<HelloWorldMessage> ReceiveHelloWorldAsync(CancellationToken stoppingToken)
        {
            var message = _queue.Take(stoppingToken);
            /*
             * simulates messages coming from another instance
             * ConsumerService ignores messages from the same instance
             */
            message.MicroserviceInstanceId = "InMemoryLocalMessaging";
            return Task.FromResult(message);
        }

        public Task SendHelloWorldAsync(HelloWorldMessage message, CancellationToken stoppingToken)
        {
            _queue.Add(message, stoppingToken);
            return Task.CompletedTask;
        }
    }
}
