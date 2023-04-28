using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.Events;
using Parameters.API.Models.Mongo;

namespace Parameters.API.Services.Mongo
{
    public class MongoService
    {
        public IMongoDatabase Database { get; }

        public MongoService(IOptions<MongoDBSettings> op, ILogger<MongoService> logger)
        {
            MongoDBSettings value = op.Value;
            if (value == null)
            {
                logger.LogError("Mongo service is not configured");
                return;
            }

            MongoClientSettings mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(value.ConnectionString));
            if (value.QueryLogging)
            {
                mongoClientSettings.ClusterConfigurator = delegate (ClusterBuilder cb)
                {
                    cb.Subscribe(delegate (CommandStartedEvent e)
                    {
                        logger.LogDebug("{CommandName} {CommandJson}", e.CommandName, e.Command);
                    });
                };
            }

            MongoClient mongoClient = new MongoClient(mongoClientSettings);
            Database = mongoClient.GetDatabase(value.DatabaseName);
        }
    }
}
