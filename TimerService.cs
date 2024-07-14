using System;
using System.Timers;

namespace MonitoringAndNotificationSystem
{
    public class TimerService : IDisposable
    {
        private readonly System.Timers.Timer timer;
        private readonly IMessagePublisher publisher;
        private readonly StatisticsCollector collector;

        public TimerService(double interval, IMessagePublisher publisher, StatisticsCollector collector)
        {
            this.publisher = publisher;
            this.collector = collector;
            timer = new System.Timers.Timer(interval) { AutoReset = true };
            timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var statistics = collector.CollectStatistics();
                Console.WriteLine($"Collected Statistics: CPU={statistics.CpuUsage}, Memory={statistics.MemoryUsage}, Available Memory={statistics.AvailableMemory}, Timestamp={statistics.Timestamp}");
                publisher.Publish(statistics);
                Console.WriteLine("Published statistics to RabbitMQ.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TimerElapsed: {ex.Message}");
            }
        }

        public void Start()
        {
            timer.Start();
            Console.WriteLine("Timer started.");
        }

        public void Stop()
        {
            timer.Stop();
            Console.WriteLine("Timer stopped.");
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
