using System;
using System.Threading;
using System.Threading.Tasks;
using HelloWorldMicroservice.Domain;
using HelloWorldMicroservice.Messaging;
using HelloWorldMicroservice.Timers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelloWorldMicroservice.Services
{
    public class ProducerService : IHostedService
    {
        private readonly ILogger<ProducerService> _logger;
        private readonly ServiceConfig _config;
        private readonly ITimer _timer;
        private readonly IHelloWorldSender _sender;

        public ProducerService(
            ILogger<ProducerService> logger,
            ServiceConfig config,
            ITimer timer,
            IHelloWorldSender sender)
        {
            _logger = logger;
            _config = config;
            _timer = timer;
            _timer.Tick += TimerTick;
            _sender = sender;
        }

        private async void TimerTick(object sender, TimerEventArgs e)
        {
            _logger.LogTrace("ProducerService.TimerTick");
            var message = new HelloWorldMessage()
            {
                MicroserviceInstanceId = _config.InstanceId,
                RequestId = Guid.NewGuid(),
                Timestamp = e.EventTime,
            };
            await _sender.SendHelloWorldAsync(message, CancellationToken.None);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("ProducerService.StartAsync");
            _timer.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("ProducerService.StopAsync");
            _timer.Stop();
            return Task.CompletedTask;
        }
    }
}
