using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Command;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Service.Queue
{

    public class GetCommandTypesMetaQueue: IRequest<ICommandArgsTypesMeta>
    {
      
        public GetCommandTypesMetaQueue(string commandName)
        {
            Command = commandName;  
        }

        public string Command { get; }
    }


    public class GetCommandTypesMetaQueueHandler : IRequestHandler<GetCommandTypesMetaQueue, ICommandArgsTypesMeta>
    {

        private readonly IMediator mediator;

        Dictionary<string, ICommandArgsTypesMeta> CommandTypesMeta { get; }


        public GetCommandTypesMetaQueueHandler(IMediator mediator)
        {
            this.mediator = mediator;
            CommandTypesMeta = new Dictionary<string, ICommandArgsTypesMeta>()
            {
                {"GetLatestPatientParams", new CommandArgsTypesMeta( new List<(Type, string)>
                {(typeof(DateTime), "startTimestamp"),(typeof(DateTime), "endTimestamp"),(typeof(int), "patientId")},
                typeof(IList<IPatientParameter>))}
            };
        }


        public async Task<ICommandArgsTypesMeta> Handle(GetCommandTypesMetaQueue request, CancellationToken cancellationToken)
        {
            if (!CommandTypesMeta.ContainsKey(request.Command))
                throw new GetCommandTypeMetaException($"Не найдено соответсвие для команды {request.Command}");
            return CommandTypesMeta[request.Command];
        }
    }
}
