﻿using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
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

        public async Task<Unit> Handle(ExecuteCodeLinesCommand request, CancellationToken cancellationToken)
        {
            foreach(string codeLine in request.Settings.CodeLines)
            {
                ICommand command = await mediator.Send(new ParseCodeLineCommand(codeLine), cancellationToken);
                await mediator.Send(new ExecuteCodeLineCommand(command, request.Settings.Properties), cancellationToken);             
            }
            return await Unit.Task;
        }
    }
}