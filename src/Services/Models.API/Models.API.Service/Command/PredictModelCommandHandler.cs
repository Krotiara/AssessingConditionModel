using MediatR;
using Models.API.Data;
using Models.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Razorvine.Pickle;
using NumSharp;
using NumSharp.Extensions;
using Numpy;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace Models.API.Service.Command
{
    public class NumpyArrayConstructor : IObjectConstructor
    {
        public object construct(object[] args)
        {
            // Extract the typecode, shape, and dtype from the arguments
            string typecode = (string)args[0];
            int[] shape = (int[])args[1];
            string dtype = (string)args[2];

            // Create a new NumSharp array with the specified typecode, shape, and dtype
            return new NDArray(Array.CreateInstance(Type.GetType(dtype), shape), shape);
        }
    }


    class NumpyReconstructConstructor : IObjectConstructor
    {
        public object construct(object[] args)
        {
            // Construct the object using the provided arguments
            return new NDArray(Array.CreateInstance(Type.GetType((string)args[2]), (int)args[1], (int)args[1]));
            //return new Numpy.np.core.multiarray._reconstruct((string)args[0], (int)args[1], (string)args[2]);
        }
    }

    public class PredictModelCommand: IRequest<float[]>
    {
        public ModelMeta ModelMeta { get; set; }

        public float[] InputArgs { get; set; }
    }

    public class PredictModelCommandHandler : IRequestHandler<PredictModelCommand, float[]>
    {
        private readonly ModelsStore _modelsStore;
        private readonly Unpickler _unpickler;

        private bool IsSupportExtension(string path) => Path.GetExtension(path) == ".onnx";

        public PredictModelCommandHandler(ModelsStore modelsStore)
        {
            _modelsStore = modelsStore;
            _unpickler = new Unpickler();
        }

        public async Task<float[]> Handle(PredictModelCommand request, CancellationToken cancellationToken)
        {
            if (!IsSupportExtension(request.ModelMeta.Name))
                return null;

            MemoryStream model = await _modelsStore.Get(request.ModelMeta.Name);
            var session = new InferenceSession(model.ToArray());
            Tensor<float> t1 = new DenseTensor<float>(request.InputArgs, new int[] { request.ModelMeta.OutputParamsCount, request.ModelMeta.InputParamsCount });
            try
            {
                return Predict(t1, session, "float_input");
            }
            catch(OnnxRuntimeException ex)
            {
                return Predict(t1, session, "X");
            }
        }


        private float[] Predict(Tensor<float> t1, InferenceSession session, string inputArgsTitle)
        {
            NamedOnnxValue t1Value = NamedOnnxValue.CreateFromTensor(inputArgsTitle, t1); //float_input - всегда один и тот же?
            using (var outPut = session.Run(new List<NamedOnnxValue> { t1Value }))
            {
                DisposableNamedOnnxValue val = outPut.First();
                DenseTensor<float> value = val.Value as DenseTensor<float>;
                return value.Buffer.ToArray();
            }
        }
    }
}
