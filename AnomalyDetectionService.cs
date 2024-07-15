namespace MonitoringAndNotificationSystem
{
    public class AnomalyDetectionService
    {
        private readonly MongoDBService _mongoDBService;
        private readonly SignalRClient _signalRClient;
        private readonly double _memoryUsageAnomalyThreshold;
        private readonly double _cpuUsageAnomalyThreshold;
        private readonly double _memoryUsageThreshold;
        private readonly double _cpuUsageThreshold;

        public AnomalyDetectionService(MongoDBService mongoDBService, SignalRClient signalRClient, double memoryUsageAnomalyThreshold, double cpuUsageAnomalyThreshold, double memoryUsageThreshold, double cpuUsageThreshold)
        {
            _mongoDBService = mongoDBService;
            _signalRClient = signalRClient;
            _memoryUsageAnomalyThreshold = memoryUsageAnomalyThreshold;
            _cpuUsageAnomalyThreshold = cpuUsageAnomalyThreshold;
            _memoryUsageThreshold = memoryUsageThreshold;
            _cpuUsageThreshold = cpuUsageThreshold;
        }

        public void ProcessStatistics(ServerStatistics statistics)
        {
            _mongoDBService.SaveStatistics(statistics);

            // Dummy implementation for previous statistics
            var previousStatistics = new ServerStatistics
            {
                MemoryUsage = statistics.MemoryUsage - 10,
                CpuUsage = statistics.CpuUsage - 10
            };

            if (statistics.MemoryUsage > previousStatistics.MemoryUsage * (1 + _memoryUsageAnomalyThreshold))
            {
                _signalRClient.SendAnomalyAlert("Memory usage anomaly detected", statistics);
            }

            if (statistics.CpuUsage > previousStatistics.CpuUsage * (1 + _cpuUsageAnomalyThreshold))
            {
                _signalRClient.SendAnomalyAlert("CPU usage anomaly detected", statistics);
            }

            if ((statistics.MemoryUsage / (statistics.MemoryUsage + statistics.AvailableMemory)) > _memoryUsageThreshold)
            {
                _signalRClient.SendHighUsageAlert("Memory usage high", statistics);
            }

            if (statistics.CpuUsage > _cpuUsageThreshold)
            {
                _signalRClient.SendHighUsageAlert("CPU usage high", statistics);
            }
        }
    }
}
