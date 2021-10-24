using HelloWorldMicroservice.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorldMicroservice.Messaging
{
    public interface IHelloWorldSender
    {
        Task SendHelloWorldAsync(HelloWorldMessage message, CancellationToken stoppingToken);
    }
}
