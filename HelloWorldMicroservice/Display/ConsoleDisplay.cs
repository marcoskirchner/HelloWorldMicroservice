using HelloWorldMicroservice.Domain;

namespace HelloWorldMicroservice.Display
{
    public class ConsoleDisplay : IDisplay
    {
        public void DisplayMessage(HelloWorldMessage message)
        {
            System.Console.WriteLine("HelloWorldMessage from {0}, id {1}, sent at {2}",
                message.MicroserviceInstanceId,
                message.RequestId,
                message.Timestamp);
        }
    }
}
