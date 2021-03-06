using HelloWorldMicroservice.Configs;
using HelloWorldMicroservice.Domain;
using HelloWorldMicroservice.Messaging;
using HelloWorldMicroservice.Services;
using HelloWorldMicroservice.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorldMicroservice.Tests
{
    [TestClass]
    public class ProducerServiceTests
    {
        [TestMethod]
        public async Task ProducerStartsTimerWhenStartingAsync()
        {
            var timer = new Mock<ITimer>();

            var producer = new ProducerService(
                Mock.Of<ILogger<ProducerService>>(),
                Mock.Of<ServiceConfig>(),
                timer.Object,
                Mock.Of<IHelloWorldSender>());
            await producer.StartAsync(CancellationToken.None);

            timer.Verify(t => t.Start());
        }

        [TestMethod]
        public async Task ProducerStopsTimerWhenStoppingAsync()
        {
            var timer = new Mock<ITimer>();

            var producer = new ProducerService(
                Mock.Of<ILogger<ProducerService>>(),
                Mock.Of<ServiceConfig>(),
                timer.Object,
                Mock.Of<IHelloWorldSender>());
            await producer.StopAsync(CancellationToken.None);

            timer.Verify(t => t.Stop());
        }

        [TestMethod]
        public void ProducerSendsMessageWhenTimerTicks()
        {
            var timer = new Mock<ITimer>();
            var sender = new Mock<IHelloWorldSender>();

            var producer = new ProducerService(
                Mock.Of<ILogger<ProducerService>>(),
                new ServiceConfig() { InstanceId = "ProducerTestInstanceId" },
                timer.Object,
                sender.Object);
            timer.Raise(t => t.Tick += null, new TimerEventArgs(DateTimeOffset.MinValue));

            sender.Verify(s => s.SendHelloWorldAsync(
                It.Is<HelloWorldMessage>(
                    m => m.MicroserviceInstanceId == "ProducerTestInstanceId"
                    && m.RequestId.Length == 8
                    && m.Timestamp == DateTimeOffset.MinValue),
                CancellationToken.None));
        }
    }
}
