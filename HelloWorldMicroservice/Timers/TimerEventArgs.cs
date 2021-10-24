using System;

namespace HelloWorldMicroservice.Timers
{
    public class TimerEventArgs : EventArgs
    {
        public TimerEventArgs(DateTimeOffset eventTime)
        {
            EventTime = eventTime;
        }

        public DateTimeOffset EventTime { get; set; }
    }
}
