using Parameters.API.Models.Mongo;
using Parameters.API.Services.Mongo;

namespace Parameters.API
{
    public static class Extensions
    {
        public static void AddMongoService(this IServiceCollection services, IConfiguration conf)
        {
            IConfigurationSection section = conf.GetSection("MongoDBSettings");
            services.Configure<MongoDBSettings>(section);
            services.AddSingleton<MongoService>();
        }
    }
}
