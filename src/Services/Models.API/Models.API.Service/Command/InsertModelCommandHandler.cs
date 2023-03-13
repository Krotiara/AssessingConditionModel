using MediatR;
using Models.API.Data;
using Models.API.Interfaces;

namespace Models.API.Service.Command
{
    public class InsertModelCommand: IRequest<Unit>
    {
        public InsertModelCommand(IModelMeta modelMeta, Stream model)
        {
            ModelMeta = modelMeta;
            Model = model;
        } 

        public IModelMeta ModelMeta { get;}

        public Stream Model { get; }
    }

    public class InsertModelCommandHandler : IRequestHandler<InsertModelCommand, Unit>
    {
        private readonly ModelsStore _modelsStore;

        public InsertModelCommandHandler(ModelsStore modelsStore)
        {
            _modelsStore = modelsStore;
        }

        public async Task<Unit> Handle(InsertModelCommand request, CancellationToken cancellationToken)
        {
            request.Model.Seek(0, SeekOrigin.Begin);
            await _modelsStore.Upload(request.Model, request.ModelMeta);
            return Unit.Value;
        }
    }
}
