using Microsoft.Extensions.Configuration;
using System.IO;

namespace MonitoringAndNotificationSystem
{
    public class AppConfig
    {
        private readonly IConfigurationRoot _configuration;

        public AppConfig()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public IConfigurationSection GetSection(string key)
        {
            return _configuration.GetSection(key);
        }

        public string GetServerIdentifier()
        {
            return _configuration.GetValue<string>("ServerStatisticsConfig:ServerIdentifier");
        }

        public int GetSamplingInterval()
        {
            return _configuration.GetValue<int>("ServerStatisticsConfig:SamplingIntervalSeconds");
        }
    }
}
