using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace MonitoringAndNotificationSystem
{
    public class RabbitMQPublisher : IMessagePublisher, IDisposable
    {
        private IModel _channel;
        private readonly string _queueName;
        private IConnection _connection;

        public RabbitMQPublisher(string hostname, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = queueName;
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void Publish(ServerStatistics statistics)
        {
            var message = JsonSerializer.Serialize(statistics);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
