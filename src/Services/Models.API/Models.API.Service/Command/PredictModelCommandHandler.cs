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

    public class PredictModelCommand: IRequest<double[]>
    {
        public ModelMeta ModelMeta { get; set; }

        public double[] InputArgs { get; set; }
    }

    public class PredictModelCommandHandler : IRequestHandler<PredictModelCommand, double[]>
    {
        private readonly ModelsStore _modelsStore;
        private readonly Unpickler _unpickler;

        public PredictModelCommandHandler(ModelsStore modelsStore)
        {
            _modelsStore = modelsStore;
            _unpickler = new Unpickler();
        }

        public async Task<double[]> Handle(PredictModelCommand request, CancellationToken cancellationToken)
        {
            MemoryStream model = await _modelsStore.Get(request.ModelMeta.Name);
            Unpickler.registerConstructor("numpy.ndarray", "NDArray", new NumpyArrayConstructor());
            Unpickler.registerConstructor("numpy.core.multiarray._reconstruct", "NDArray", new NumpyReconstructConstructor()); #error here
            object obj = _unpickler.load(model);
           // ITransformer trainedModel = _mLContext.Model.Load(model, out DataViewSchema modelSchema);

            throw new NotImplementedException();

        }
    }
}
