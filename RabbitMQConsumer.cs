using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace MonitoringAndNotificationSystem
{
    public class RabbitMQConsumer
    {
        private readonly string hostname;
        private readonly string queueName;

        public RabbitMQConsumer(string hostname, string queueName)
        {
            this.hostname = hostname;
            this.queueName = queueName;
        }

        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            try
            {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                Console.WriteLine($"Connecting to RabbitMQ at {hostname}");
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                Console.WriteLine($"Connected to RabbitMQ and declared queue {queueName}");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var statistics = JsonSerializer.Deserialize<ServerStatistics>(message);

                    Console.WriteLine($"Received message: {message}");
                    Console.WriteLine($"Processed message at {statistics.Timestamp}");
                    Console.WriteLine($"CPU Usage: {statistics.CpuUsage}%");
                    Console.WriteLine($"Memory Usage: {statistics.MemoryUsage} MB");
                    Console.WriteLine($"Available Memory: {statistics.AvailableMemory} MB");
                    Console.WriteLine();
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                Console.WriteLine("Consumer started. Press [enter] to exit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

