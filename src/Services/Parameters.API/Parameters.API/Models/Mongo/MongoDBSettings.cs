namespace Parameters.API.Models.Mongo
{
    public class MongoDBSettings
    {
        public MongoDBSettings() { }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public bool QueryLogging { get; set; }
    }
}
