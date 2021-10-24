using System;

namespace HelloWorldMicroservice.Domain
{
    public class HelloWorldMessage
    {
        public string MicroserviceInstanceId { get; set; }
        public Guid RequestId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    };
}
