using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace MonitoringAndNotificationSystem
{
    public class SignalRClient
    {
        private readonly HubConnection _connection;

        public SignalRClient(string url)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            _connection.StartAsync().Wait();
        }

        public void SendAnomalyAlert(string message, ServerStatistics statistics)
        {
            SendAlert("AnomalyAlert", message, statistics);
        }

        public void SendHighUsageAlert(string message, ServerStatistics statistics)
        {
            SendAlert("HighUsageAlert", message, statistics);
        }

        private void SendAlert(string methodName, string message, ServerStatistics statistics)
        {
            var alert = new
            {
                Message = message,
                ServerIdentifier = statistics.ServerIdentifier,
                MemoryUsage = statistics.MemoryUsage,
                AvailableMemory = statistics.AvailableMemory,
                CpuUsage = statistics.CpuUsage,
                Timestamp = statistics.Timestamp
            };

            _connection.InvokeAsync(methodName, alert).Wait();
        }
    }
}
