using HelloWorldMicroservice.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorldMicroservice.Messaging
{
    public interface IHelloWorldReceiver
    {
        Task<HelloWorldMessage> ReceiveHelloWorldAsync(CancellationToken stoppingToken);
    }
}
