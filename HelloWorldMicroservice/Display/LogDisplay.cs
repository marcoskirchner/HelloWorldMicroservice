using HelloWorldMicroservice.Domain;
using Microsoft.Extensions.Logging;

namespace HelloWorldMicroservice.Display
{
    public class LogDisplay : IDisplay
    {
        private readonly ILogger<LogDisplay> _logger;

        public LogDisplay(ILogger<LogDisplay> logger)
        {
            _logger = logger;
        }

        public void DisplayMessage(HelloWorldMessage message)
        {
            _logger.LogInformation("HelloWorldMessage from {0}, id {1}, sent at {2}",
                message.MicroserviceInstanceId,
                message.RequestId,
                message.Timestamp);
        }
    }
}
