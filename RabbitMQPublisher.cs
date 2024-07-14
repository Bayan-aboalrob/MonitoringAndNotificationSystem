using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace MonitoringAndNotificationSystem
{
    public class RabbitMQPublisher : IMessagePublisher, IDisposable
    {
        private readonly IModel channel;
        private readonly IConnection connection;
        private readonly string queueName;

        public RabbitMQPublisher(string hostname, string queue)
        {
            Console.WriteLine($"Connecting to RabbitMQ at {hostname}");

            var factory = new ConnectionFactory() { HostName = hostname };
            try
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                queueName = queue;
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                Console.WriteLine($"Connected to RabbitMQ and declared queue {queueName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not connect to RabbitMQ: {ex.Message}");
                throw;
            }
        }

        public void Publish(ServerStatistics statistics)
        {
            try
            {
                var message = JsonSerializer.Serialize(statistics);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                Console.WriteLine($"Published message to queue {queueName}: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error publishing message: {ex.Message}");
            }
        }

        public void Dispose()
        {
            channel?.Close();
            channel?.Dispose();
            connection?.Close();
            connection?.Dispose();
        }
    }
}
