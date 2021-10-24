using System;
using System.Threading;
using System.Threading.Tasks;
using HelloWorldMicroservice.Display;
using HelloWorldMicroservice.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelloWorldMicroservice.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly ILogger<ConsumerService> _logger;
        private readonly ServiceConfig _config;
        private readonly IHelloWorldReceiver _receiver;
        private readonly IDisplay _display;

        public ConsumerService(
            ILogger<ConsumerService> logger,
            ServiceConfig config,
            IHelloWorldReceiver receiver,
            IDisplay display)
        {
            _logger = logger;
            _config = config;
            _receiver = receiver;
            _display = display;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("ConsumerService.StartAsync");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("ConsumerService.StopAsync");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogTrace("ConsumerService.ExecuteAsync");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var message = await _receiver.ReceiveHelloWorldAsync(stoppingToken);
                    if (message.MicroserviceInstanceId != _config.InstanceId)
                    {
                        _display.DisplayMessage(message);
                    }
                }
                catch (OperationCanceledException) { };
            }
        }
    }
}
