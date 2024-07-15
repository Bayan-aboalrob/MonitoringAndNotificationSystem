using System;
using System.Diagnostics;

namespace MonitoringAndNotificationSystem
{
    public class StatisticsCollector
    {
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _availableMemoryCounter;
        private PerformanceCounter _totalMemoryCounter;

        public StatisticsCollector()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");
            _totalMemoryCounter = new PerformanceCounter("Memory", "Committed Bytes");
        }

        public ServerStatistics CollectStatistics()
        {
            var availableMemory = _availableMemoryCounter.NextValue();
            var totalMemory = _totalMemoryCounter.NextValue() / (1024 * 1024); // Convert from bytes to MB

            return new ServerStatistics
            {
                MemoryUsage = (totalMemory - availableMemory),
                AvailableMemory = availableMemory,
                CpuUsage = _cpuCounter.NextValue(),
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
