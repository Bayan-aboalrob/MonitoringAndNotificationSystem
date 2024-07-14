using System;

namespace MonitoringAndNotificationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("consumer", StringComparison.OrdinalIgnoreCase))
            {
                RunConsumer();
            }
            else
            {
                RunPublisher();
            }
        }

        static void RunPublisher()
        {
            try
            {
                var config = new AppConfig();
                var hostname = "localhost";
                var queueName = "ServerStatistics." + config.GetServerIdentifier(); 
                var interval = config.GetSamplingInterval() * 1000; 

                Console.WriteLine($"Hostname: {hostname}");
                Console.WriteLine($"Queue Name: {queueName}");
                Console.WriteLine($"Sampling Interval: {interval}ms");

                using (var publisher = new RabbitMQPublisher(hostname, queueName))
                {
                    var collector = new StatisticsCollector();
                    using (var timerService = new TimerService(interval, publisher, collector))
                    {
                        Console.WriteLine("Monitoring started. Press any key to stop.");
                        timerService.Start();
                        Console.ReadLine();
                        timerService.Stop();
                        Console.WriteLine("Monitoring stopped.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RunPublisher: {ex.Message}");
            }
        }

        static void RunConsumer()
        {
            try
            {
                var config = new AppConfig();
                var hostname = "localhost";
                var queueName = "ServerStatistics." + config.GetServerIdentifier();

                var consumer = new RabbitMQConsumer(hostname, queueName);
                consumer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RunConsumer: {ex.Message}");
            }
        }
    }
}

