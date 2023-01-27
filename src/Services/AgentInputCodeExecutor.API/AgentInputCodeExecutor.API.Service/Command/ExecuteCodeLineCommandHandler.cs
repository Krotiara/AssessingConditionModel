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

        public Dictionary<ParameterNames, AgentProperty> Properties { get; }

        public Dictionary<string, object> LocalVariables { get; }

        public ExecuteCodeLineCommand(ICommand command, Dictionary<ParameterNames, AgentProperty> properties, Dictionary<string, object> localVariables)
        {
            Command = command;
            Properties = properties;
            LocalVariables = localVariables;
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

#warning Не доделано. Главная проблема - запихнуть в expression асинхронную функцию. Или найти другой способ исполнения. 
            (ICommandArgsTypesMeta, Delegate) commandPair = await codeResolveService.ResolveCommandAction(request.Command);
            List<object> variables = await mediator.Send(new GetCommandArgsValuesQueue(request.Command.OriginCommand, commandPair.Item1, request.LocalVariables));
            if (request.Command.CommandType == CommandType.Assigning)
            {
                object res = commandPair.Item2.DynamicInvoke(variables);
                TypeConverter typeConverter = TypeDescriptor.GetConverter(commandPair.Item1.OutputArgType);
                var convertedRes = typeConverter.ConvertFrom(res);
#warning Лучше заносить значения. которые содержатся в мете информации - имена аргументов. Но не факт, Нужно думать.
#warning Забытая установка Properties - нужно останавливать расчитанные параметры агентов.
                request.LocalVariables[request.Command.AssigningParamOriginalName] = convertedRes;
            }
            else
            {
                commandPair.Item2.DynamicInvoke(variables);
            }

            return await Unit.Task;
        }
    }
}
