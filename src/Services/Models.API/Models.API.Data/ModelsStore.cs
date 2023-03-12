using Models.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Data
{
    public class ModelsStore
    {
        private readonly S3ClientService _s3Client;

        public ModelsStore(S3ClientService s3Client)
        {
            _s3Client = s3Client;
        }
    }
}
