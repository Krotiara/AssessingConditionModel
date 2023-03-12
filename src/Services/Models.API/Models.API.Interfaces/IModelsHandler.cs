using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Interfaces
{
    public interface IModelsHandler
    {

        public Task InsertModel(Stream model, IModelMeta meta);

        public Task<IModelMeta> GetModelMeta(string modelId);

        public Task<double[]> PredictAsync(string modelId, double[] args);
    }
}
