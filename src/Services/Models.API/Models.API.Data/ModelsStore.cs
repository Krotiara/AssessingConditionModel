using Minio;
using Minio.DataModel.Tags;
using Models.API.Entities;
using Models.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

    
        public async Task Upload(Stream model, IModelMeta meta)
        {
            var tags = new Dictionary<string, string>
            {
                { "ModelTag", "Model" }
            };
           
            PutObjectArgs args = new Minio.PutObjectArgs()
                .WithBucket(_s3Client.Bucket)
                .WithObject(meta.Name)
                .WithStreamData(model)
                .WithObjectSize(model.Length)
                .WithRequestBody(null)
                .WithContentType("application/octet-stream")
                .WithTagging(Tagging.GetObjectTags(tags))
                .WithVersionId(meta.Version.ToString());

            await _s3Client.Client.PutObjectAsync(args).ConfigureAwait(false);
        }


        public async Task<MemoryStream> Get(string fileName)
        {
            MemoryStream memoryStream = new MemoryStream();
            GetObjectArgs args = new GetObjectArgs()
               .WithBucket(_s3Client.Bucket)
               .WithObject(fileName)
               .WithCallbackStream(async stream => await stream.CopyToAsync(memoryStream).ConfigureAwait(false));
            await _s3Client.Client.GetObjectAsync(args).ConfigureAwait(false);
            return memoryStream;
        }


       
    }
}
