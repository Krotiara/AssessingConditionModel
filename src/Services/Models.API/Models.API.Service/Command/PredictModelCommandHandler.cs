using MediatR;
using Models.API.Data;
using Models.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Razorvine.Pickle;

namespace Models.API.Service.Command
{
  
    public class PredictModelCommand: IRequest<double[]>
    {
        public ModelMeta ModelMeta { get; set; }

        public double[] InputArgs { get; set; }
    }

    public class PredictModelCommandHandler : IRequestHandler<PredictModelCommand, double[]>
    {
        private readonly ModelsStore _modelsStore;
        private readonly Unpickler _mLContext;

        public PredictModelCommandHandler(ModelsStore modelsStore)
        {
            _modelsStore = modelsStore;
            _mLContext = new Unpickler();
        }

        public async Task<double[]> Handle(PredictModelCommand request, CancellationToken cancellationToken)
        {
            var model = await _modelsStore.Get(request.ModelMeta.Name);

            object result = _mLContext.load(model);

            throw new NotImplementedException();

        }
    }
}
