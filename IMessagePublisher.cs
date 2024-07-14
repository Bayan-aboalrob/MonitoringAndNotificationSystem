namespace MonitoringAndNotificationSystem
{
    public interface IMessagePublisher
    {
        void Publish(ServerStatistics statistics);
    }
}
