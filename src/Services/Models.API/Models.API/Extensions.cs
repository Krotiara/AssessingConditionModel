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
    }
}
