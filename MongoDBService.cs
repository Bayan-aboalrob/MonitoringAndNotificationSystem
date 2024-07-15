using MongoDB.Driver;
using System;

namespace MonitoringAndNotificationSystem
{
    public class MongoDBService
    {
        private readonly IMongoCollection<ServerStatistics> _collection;

        public MongoDBService(string connectionString, string databaseName, string collectionName)
        {
            try
            {
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase(databaseName);
                _collection = database.GetCollection<ServerStatistics>(collectionName);
                Console.WriteLine("MongoDB connection established.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MongoDB: {ex.Message}");
            }
        }

        public void SaveStatistics(ServerStatistics statistics)
        {
            try
            {
                _collection.InsertOne(statistics);
                Console.WriteLine("Statistics saved to MongoDB.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving statistics to MongoDB: {ex.Message}");
            }
        }
    }
}
