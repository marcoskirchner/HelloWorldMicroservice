using System;
using System.Threading;
using System.Threading.Tasks;
using HelloWorldMicroservice.Configs;
using HelloWorldMicroservice.Display;
using HelloWorldMicroservice.Domain;
using HelloWorldMicroservice.Messaging;
using HelloWorldMicroservice.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HelloWorldMicroservice.Tests
{
    [TestClass]
    public class ConsumerServiceTests
    {
        [TestMethod]
        public async Task ConsumerDispatchesMessageFromAnotherInstance()
        {

            var sentMessage = new HelloWorldMessage()
            {
                MicroserviceInstanceId = "ConsumerTestInstanceId1",
                RequestId = Guid.NewGuid().ToString().Split('-')[0],
                Timestamp = DateTimeOffset.Now,
            };
            var receiver = new Mock<IHelloWorldReceiver>();
            receiver.Setup(r => r.ReceiveHelloWorldAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(sentMessage);

            var cts = new CancellationTokenSource();
            var display = new Mock<IDisplay>(MockBehavior.Strict);
            display.Setup(d => d.DisplayMessage(sentMessage)).
                Callback(() => cts.Cancel())
                .Verifiable();

            var consumer = new ConsumerService(
                Mock.Of<ILogger<ConsumerService>>(),
                new ServiceConfig() { InstanceId = "ConsumerTestInstanceId0" },
                receiver.Object,
                display.Object);
            await consumer.StartAsync(cts.Token);

            display.Verify();
        }

        [TestMethod]
        public async Task ConsumerDoesNotDispatchMessageFromSameInstance()
        {
            var sentMessage = new HelloWorldMessage()
            {
                MicroserviceInstanceId = "ConsumerTestInstanceId0",
                RequestId = Guid.NewGuid().ToString().Split('-')[0],
                Timestamp = DateTimeOffset.Now,
            };
            var receiver = new Mock<IHelloWorldReceiver>();
            var cts = new CancellationTokenSource();
            receiver.Setup(r => r.ReceiveHelloWorldAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(sentMessage)
                .Callback(() => cts.Cancel());

            var display = new Mock<IDisplay>(MockBehavior.Strict);

            var consumer = new ConsumerService(
                Mock.Of<ILogger<ConsumerService>>(),
                new ServiceConfig() { InstanceId = "ConsumerTestInstanceId0" },
                receiver.Object,
                display.Object);
            await consumer.StartAsync(cts.Token);

            display.Verify();
        }
    }
}
