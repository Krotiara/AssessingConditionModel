using Agents.API.Entities;
using Agents.API.Interfaces;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{

    public class GetCommandTypesMetaQueue: IRequest<ICommandArgsTypesMeta>
    {
      
        public GetCommandTypesMetaQueue(SystemCommands commandName)
        {
            Command = commandName;  
        }

        public SystemCommands Command { get; }
    }


    public class GetCommandTypesMetaQueueHandler : IRequestHandler<GetCommandTypesMetaQueue, ICommandArgsTypesMeta>
    {

        private readonly IMediator mediator;
        private readonly IMetaStorageService metaStorageService;

        public GetCommandTypesMetaQueueHandler(IMediator mediator, IMetaStorageService metaStorageService)
        {
            this.metaStorageService = metaStorageService;
        }


        public async Task<ICommandArgsTypesMeta> Handle(GetCommandTypesMetaQueue request, CancellationToken cancellationToken)
        {
            ICommandArgsTypesMeta? meta = metaStorageService.GetMetaByCommandName(request.Command);
            if(meta == null)
                throw new GetCommandTypeMetaException($"Не найдено соответсвие для команды {request.Command}");
            return meta;
        }
    }
}
