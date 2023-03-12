using Models.API.Entities;
using Models.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Data
{
    public class ModelsStore: IModelsHandler
    {
        private readonly S3ClientService _s3Client;

        public ModelsStore(S3ClientService s3Client)
        {
            _s3Client = s3Client;
        }

        public Task<IModelMeta> GetModelMeta(string modelId)
        {
            throw new NotImplementedException();
        }

        public async Task InsertModel(Stream model, IModelMeta meta)
        {
            await _s3Client.Client.PutObjectAsync(new Minio.PutObjectArgs()
                .WithBucket(_s3Client.Bucket)
                .WithStreamData(model)
                .WithFileName(meta.Name));
            throw new NotImplementedException();
        }

        public Task<double[]> PredictAsync(string modelId, double[] args)
        {
            throw new NotImplementedException();
        }
    }
}
