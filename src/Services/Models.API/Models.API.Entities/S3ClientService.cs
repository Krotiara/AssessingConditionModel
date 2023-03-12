using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Entities
{
    public class S3ClientService
    {
        public MinioClient Client { get; }

        public S3ClientService(IOptions<S3StorageSettings> options, ILogger<S3ClientService> logger)
        { 
            if (options.Value == null) return;
            S3StorageSettings sets = options.Value;
            Client = new MinioClient()
                .WithEndpoint(sets.URL)
                .WithCredentials(sets.AccessKey,sets.SecretKey)
                .WithSSL()
                .Build();
        }
    }
}
