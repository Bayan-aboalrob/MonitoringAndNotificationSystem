using System;
using System.Diagnostics;

namespace MonitoringAndNotificationSystem
{
    public class StatisticsCollector
    {
        private PerformanceCounter cpuCounter;
        private PerformanceCounter availableMemoryCounter;
        private PerformanceCounter totalMemoryCounter;

        public StatisticsCollector()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");
            totalMemoryCounter = new PerformanceCounter("Memory", "Committed Bytes");
        }

        public ServerStatistics CollectStatistics()
        {
            var availableMemory = availableMemoryCounter.NextValue();
            var totalMemory = totalMemoryCounter.NextValue() / (1024 * 1024); 

            return new ServerStatistics
            {
                MemoryUsage = (totalMemory - availableMemory),
                AvailableMemory = availableMemory,
                CpuUsage = cpuCounter.NextValue(),
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
