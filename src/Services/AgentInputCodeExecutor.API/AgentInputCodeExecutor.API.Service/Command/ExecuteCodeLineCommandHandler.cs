using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Queue;
using Interfaces;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Service.Command
{

    public class ExecuteCodeLineCommand: IRequest
    {
        public ICommand Command { get; }

        public ExecuteCodeLineCommand(ICommand command)
        {
            Command = command;
        }
    }

    public class ExecuteCodeLineCommandHandler : IRequestHandler<ExecuteCodeLineCommand, Unit>
    {

        private readonly ICodeResolveService codeResolveService;
        private readonly IMediator mediator;

        public ExecuteCodeLineCommandHandler(ICodeResolveService codeResolveService, IMediator mediator)
        {
            this.codeResolveService = codeResolveService;
            this.mediator = mediator;
        }


        public async Task<Unit> Handle(ExecuteCodeLineCommand request, CancellationToken cancellationToken)
        {

            (ICommandArgsTypesMeta, Delegate) commandPair = await codeResolveService.ResolveCommandAction(request.Command);
            List<object> variables = await mediator.Send(new GetCommandArgsValuesQueue(request.Command, commandPair.Item1), cancellationToken);
            if (request.Command.CommandType == CommandType.Assigning)
            {
                object res = commandPair.Item2.DynamicInvoke(variables.ToArray());
                TypeConverter typeConverter = TypeDescriptor.GetConverter(commandPair.Item1.OutputArgType);
                var convertedRes = typeConverter.ConvertTo(res, commandPair.Item1.OutputArgType);

                if (request.Command.LocalVariables.ContainsKey(request.Command.AssigningParamOriginalName))
                    request.Command.LocalVariables[request.Command.AssigningParamOriginalName].Value = convertedRes;
                else
                {
                    request.Command.LocalVariables[request.Command.AssigningParamOriginalName] =
                        new AgentProperty(request.Command.AssigningParameter, commandPair.Item1.OutputArgType, convertedRes, request.Command.AssigningParamOriginalName);
                }
            }
            else
            {
                //На случай вызовов коанд, которые не возвращают значение.
                commandPair.Item2.DynamicInvoke(variables);
            }

            return await Unit.Task;
        }
    }
}
