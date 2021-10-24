using System;

namespace HelloWorldMicroservice.Domain
{
    public class HelloWorldMessage
    {
        public string MicroserviceInstanceId;
        public Guid RequestId;
        public DateTimeOffset Timestamp;
    };
}
