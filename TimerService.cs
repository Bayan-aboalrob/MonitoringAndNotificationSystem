using System;
using System.Timers;

namespace MonitoringAndNotificationSystem
{
    public class TimerService : IDisposable
    {
        private readonly System.Timers.Timer _timer;
        private readonly IMessagePublisher _publisher;
        private readonly StatisticsCollector _collector;

        public TimerService(double interval, IMessagePublisher publisher, StatisticsCollector collector)
        {
            _publisher = publisher;
            _collector = collector;
            _timer = new System.Timers.Timer(interval) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var statistics = _collector.CollectStatistics();
            _publisher.Publish(statistics);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
