using AgentInputCodeExecutor.API.Entities;
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
        public ExecutableAgentCodeSettings Settings { get; set; }
    }

    public class ExecuteCodeLinesCommandHandler : IRequestHandler<ExecuteCodeLinesCommand, Unit>
    {
        private readonly IMediator mediator;

        public ExecuteCodeLinesCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<Unit> Handle(ExecuteCodeLinesCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
