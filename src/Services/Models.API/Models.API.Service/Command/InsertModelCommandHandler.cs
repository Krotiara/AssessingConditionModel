using MediatR;
using Models.API.Data;
using Models.API.Entities;
using Models.API.Interfaces;

namespace Models.API.Service.Command
{
    public class InsertModelCommand: IRequest<Unit>
    {
        public InsertModelCommand(UploadModel modelMeta, Stream model)
        {
            ModelMeta = modelMeta;
            Model = model;
        } 

        public UploadModel ModelMeta { get;}

        public Stream Model { get; }
    }

    public class InsertModelCommandHandler : IRequestHandler<InsertModelCommand, Unit>
    {
        private readonly ModelsStore _modelsStore;
        private readonly ModelsMetaStore _modelsMetaStore;

        public InsertModelCommandHandler(ModelsStore modelsStore, ModelsMetaStore modelsMetaStore)
        {
            _modelsStore = modelsStore;
            _modelsMetaStore = modelsMetaStore;
        }

        public async Task<Unit> Handle(InsertModelCommand request, CancellationToken cancellationToken)
        {
            request.Model.Seek(0, SeekOrigin.Begin);
            await _modelsMetaStore.Insert(new ModelMeta()
            {
                Id = request.ModelMeta.Id,
                File = request.ModelMeta.File,
                Accuracy = request.ModelMeta.Accuracy,
                InputParamsCount = request.ModelMeta.InputParamsCount,
                OutputParamsCount = request.ModelMeta.OutputParamsCount,
                Version = request.ModelMeta.Version,
                ParamsNames = request.ModelMeta.ParamsNames
            });
            await _modelsStore.Upload(request.Model, request.ModelMeta);
            return Unit.Value;
        }
    }
}
