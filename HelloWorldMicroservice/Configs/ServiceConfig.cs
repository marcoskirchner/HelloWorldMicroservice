namespace HelloWorldMicroservice.Configs
{
    public class ServiceConfig
    {
        public string InstanceId { get; set; }
        public int TimerInterval { get; set; }
        public int StartupDelayInterval { get; set; }
        public string MessagingImplementation { get; set; }
    }
}
