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
            (ICommandArgsTypesMeta, object) commandPair = await codeResolveService.ResolveCommandAction(request.Command);
            List<object> variables = await mediator.Send(new GetCommandArgsValuesQueue(request.Command.OriginCommand, commandPair.Item1, request.LocalVariables));

            List<Type> types = new(commandPair.Item1.InputArgsTypes);
            if (request.Command.CommandType == CommandType.Assigning)
            {
                types.Add(commandPair.Item1.OutputArgType);
                Type funcType = Expression.GetFuncType(types.ToArray());
                List<ParameterExpression> parameters = new();
                foreach (Type type in commandPair.Item1.InputArgsTypes)
                    parameters.Add(Expression.Parameter(type));

                LambdaExpression lambda = parameters.Count > 0 ?
                    Expression.Lambda(funcType,/* Expression.Constant(true),*/ parameters) :
                    Expression.Lambda(funcType, /*Expression.Constant(true)*/);
                object res = lambda.Compile().DynamicInvoke(variables);
                TypeConverter typeConverter = TypeDescriptor.GetConverter(commandPair.Item1.OutputArgType);
                var convertedRes = typeConverter.ConvertFrom(res);
                request.LocalVariables[request.Command.AssigningParamOriginalName] = convertedRes;
            }
            else
            {
                Type funcType = Expression.GetActionType(types.ToArray());
                List<ParameterExpression> parameters = new();
                foreach (Type type in commandPair.Item1.InputArgsTypes)
                    parameters.Add(Expression.Parameter(type));
                LambdaExpression lambda = parameters.Count > 0 ?
                    Expression.Lambda(funcType, /*Expression.Constant(true)*/, parameters) :
                    Expression.Lambda(funcType, /*Expression.Constant(true)*/);
                Expression.Lambda()
            }
//#warning Перед этим нужно преобразование функции из псевдокода в исполняемый внутренний код, где всякие запросы на микросервисы и прочее.
//            ScriptState<object> scriptState = await CSharpScript.RunAsync(request.Command.OriginCommand, cancellationToken: cancellationToken);
//            if(scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
//            {
//                if (request.Command.CommandType == CommandType.Assigning)
//                    request.Properties[request.Command.AssigningParameter].Value = scriptState.ReturnValue;
//            }
//            return await Unit.Task;
        }
    }
}
