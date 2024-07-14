using Microsoft.Extensions.Configuration;
using System.IO;

namespace MonitoringAndNotificationSystem
{
    public class AppConfig
    {
        public IConfiguration Configuration { get; }

        public AppConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public int GetSamplingInterval() => Configuration.GetValue<int>("ServerStatisticsConfig:SamplingIntervalSeconds");
        public string GetServerIdentifier() => Configuration.GetValue<string>("ServerStatisticsConfig:ServerIdentifier");
    }
}
