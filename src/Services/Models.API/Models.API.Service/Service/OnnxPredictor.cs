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
    public class OnnxPredictor : IPredictor
    {
        public double[] Predict(string modelPath, double[] args, int[] dimensions)
        {
            var session = new InferenceSession(modelPath);
            Tensor<float> t1 = new DenseTensor<float>(args.Cast<float>().ToArray(), dimensions);
            NamedOnnxValue t1Value = NamedOnnxValue.CreateFromTensor("float_input", t1);
            using (var outPut = session.Run(new List<NamedOnnxValue> { t1Value }))
            {
                DisposableNamedOnnxValue bioAgeOV = outPut.First();
                DenseTensor<float> value = bioAgeOV.Value as DenseTensor<float>;
                return value
                    .Buffer.ToArray()
                    .Cast<double>()
                    .ToArray();
            }
        }
    }
}
