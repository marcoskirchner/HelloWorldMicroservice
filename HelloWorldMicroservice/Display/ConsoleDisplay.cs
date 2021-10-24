using System;
using HelloWorldMicroservice.Configs;
using HelloWorldMicroservice.Domain;

namespace HelloWorldMicroservice.Display
{
    public class ConsoleDisplay : IDisplay
    {
        private readonly ServiceConfig _config;

        public ConsoleDisplay(ServiceConfig config)
        {
            _config = config;
        }

        public void DisplayMessage(HelloWorldMessage message)
        {
            Console.WriteLine("[{0}]: HelloWorldMessage from {1}, id {2}, sent at {3}",
                _config.InstanceId,
                message.MicroserviceInstanceId,
                message.RequestId,
                message.Timestamp);
        }
    }
}
