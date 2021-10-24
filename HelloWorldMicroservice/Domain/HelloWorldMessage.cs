using System;

namespace HelloWorldMicroservice.Domain
{
    public class HelloWorldMessage
    {
        public string MicroserviceInstanceId { get; set; }
        public string RequestId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    };
}
