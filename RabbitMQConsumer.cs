using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace MonitoringAndNotificationSystem
{
    public class RabbitMQConsumer
    {
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly AnomalyDetectionService _anomalyDetectionService;

        public RabbitMQConsumer(string hostname, string queueName, AnomalyDetectionService anomalyDetectionService)
        {
            _hostname = hostname;
            _queueName = queueName;
            _anomalyDetectionService = anomalyDetectionService;
        }

        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var statistics = JsonSerializer.Deserialize<ServerStatistics>(message);

                Console.WriteLine($"Received message: {message}");
                _anomalyDetectionService.ProcessStatistics(statistics);
            };

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            Console.WriteLine("Consumer started. Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
