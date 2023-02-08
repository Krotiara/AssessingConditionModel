using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Service.Command
{

    public class ExecuteCodeLinesCommand: IRequest
    {
        public ExecuteCodeLinesCommand(ExecutableAgentCodeSettings settings)
        {
            Settings = settings;
        }

        public ExecutableAgentCodeSettings Settings { get;}
    }

    public class ExecuteCodeLinesCommandHandler : IRequestHandler<ExecuteCodeLinesCommand, Unit>
    {
        private readonly IMediator mediator;

        public Dictionary<string, IProperty> LocalVariables { get; private set; }

        public ExecuteCodeLinesCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
            LocalVariables = new Dictionary<string, IProperty>();
        }

        public async Task<Unit> Handle(ExecuteCodeLinesCommand request, CancellationToken cancellationToken)
        {
            LocalVariables = request.Settings.Properties; //TODO Сейчас прокидывается через settings. По идее можно убрать прокидывание и оставить только здесь инициализацию.
            foreach (string codeLine in request.Settings.CodeLines)
            {
                ICommand command = await mediator.Send(new ParseCodeLineCommand(codeLine, LocalVariables), cancellationToken);
                await mediator.Send(new ExecuteCodeLineCommand(command), cancellationToken);             
            }
            return await Unit.Task;
        }
    }
}
