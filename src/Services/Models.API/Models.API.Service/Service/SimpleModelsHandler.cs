using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Models.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Service.Service
{
    public class SimpleModelsHandler : IModelsHandler
    {
        public IModelMeta GetModelMeta(string modelId)
        {
            throw new NotImplementedException();
        }

        public Task<double[]> PredictAsync(string modelId, double[] args)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Тестовый вариант
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private string ResolveModelPathById(string modelId)
        {
            return modelId switch
            {
                "bioAge" => Path.Combine(Directory.GetCurrentDirectory(), @"Resources/bioAgeFuncModel.onnx"),
                _ => null,
            };
        }


        /// <summary>
        /// Тестовый вариант
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private IPredictor ResolvePredictorById(string modelId)
        {
            return modelId switch
            {
                "bioAge" => new OnnxPredictor(),
                _ => null,
            };
        }
    }
}
