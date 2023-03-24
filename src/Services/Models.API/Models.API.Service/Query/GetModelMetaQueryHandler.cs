using MediatR;
using Models.API.Data;
using Models.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Service.Query
{
    public class GetModelMetaQuery: IRequest<ModelMeta>
    {
        public string ModelId { get; set; }

        public string Version { get; set; }
    }

    public class GetModelMetaQueryHandler : IRequestHandler<GetModelMetaQuery, ModelMeta>
    {
        private readonly ModelsMetaStore _modelsMetaStore;

        public GetModelMetaQueryHandler(ModelsMetaStore modelsMetaStore)
        {
            _modelsMetaStore = modelsMetaStore;
        }

        public async Task<ModelMeta> Handle(GetModelMetaQuery request, CancellationToken cancellationToken)
        {
            return await _modelsMetaStore.Get(request.ModelId, request.Version);
        }
    }
}