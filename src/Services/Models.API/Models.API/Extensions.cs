using Microsoft.EntityFrameworkCore;
using Models.API.Data;
using Models.API.Entities;

namespace Models.API
{
    public static class Extensions
    {
        public static void AddS3ClientService(this IServiceCollection services, IConfiguration conf)
        {
            IConfigurationSection section = conf.GetSection("S3Settings");
            services.Configure<S3StorageSettings>(section);
            services.AddTransient<S3ClientService>();
        }

        public static void AddPostgresService(this IServiceCollection services, IConfiguration conf)
        {
            string connectionString = conf.GetConnectionString("PostgresConnection");
            services.AddDbContext<ModelsMetaDbContext>(options => 
                options.UseNpgsql(connectionString, builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null)));
            services.AddTransient<ModelsMetaStore>();
        }
    }
}
