using HelloWorldMicroservice.Domain;

namespace HelloWorldMicroservice.Display
{
    public interface IDisplay
    {
        void DisplayMessage(HelloWorldMessage message);
    }
}
