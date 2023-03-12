using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Interfaces
{
    public interface IModelsHandler
    {
        public IModelMeta GetModelMeta(string modelId);

        public Task<double[]> PredictAsync(string modelId, double[] args);
    }
}
