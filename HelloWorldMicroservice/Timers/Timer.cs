using System;
using HelloWorldMicroservice.Configs;

namespace HelloWorldMicroservice.Timers
{
    public class Timer : ITimer, IDisposable
    {
        private readonly System.Timers.Timer _internalTimer;

        public Timer(ServiceConfig config)
        {
            _internalTimer = new System.Timers.Timer(config.TimerInterval);
            _internalTimer.Elapsed += (s, e) =>
            {
                Tick?.Invoke(this, new(DateTimeOffset.Now));
            };
        }

        public event EventHandler<TimerEventArgs> Tick;

        public void Start()
        {
            _internalTimer.Start();
        }

        public void Stop()
        {
            _internalTimer.Stop();
        }

        public void Dispose()
        {
            _internalTimer.Stop();
            _internalTimer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
