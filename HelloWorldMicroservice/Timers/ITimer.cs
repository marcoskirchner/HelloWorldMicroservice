using System;

namespace HelloWorldMicroservice.Timers
{
    public interface ITimer
    {
        public event EventHandler<TimerEventArgs> Tick;
        void Start();
        void Stop();
    }
}