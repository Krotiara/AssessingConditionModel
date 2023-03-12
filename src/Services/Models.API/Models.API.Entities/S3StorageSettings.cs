using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Entities
{
    public class S3StorageSettings
    {
        public string URL { get; set; }

        public string Bucket { get; set; }

        public string Region { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }
    }
}
