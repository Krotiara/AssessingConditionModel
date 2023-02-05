using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Queue;
using Interfaces;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
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
            //Случай простых команд присвоения или расчета без вызова функции.
            if(request.Command.CommandType == CommandType.Assigning && !IsContainsCommandCall(request.Command.OriginCommand))
            {
                ExecuteCommandWithoutCommandCall(request);
                return await Unit.Task;
            }

#warning Не учтен случай, когда есть и вызов функции, и простые слагаемые. Нужно доьавить обнаружение этого и эксепшн. Усложнять псевдо-выполнитель кода не надо.

            (ICommandArgsTypesMeta, Delegate) commandPair = await codeResolveService.ResolveCommandAction(request.Command, cancellationToken);
            List<object> variables = await mediator.Send(new GetCommandArgsValuesQueue(request.Command, commandPair.Item1), cancellationToken);

            if (request.Command.CommandType == CommandType.Assigning)
            {
                object res = commandPair.Item2.DynamicInvoke(variables.ToArray());
                if (commandPair.Item1.OutputArgType.GetInterface(nameof(IEnumerable)) == null)
                {
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(commandPair.Item1.OutputArgType);
                    res = typeConverter.ConvertTo(res, commandPair.Item1.OutputArgType);
                }
               
                if (request.Command.LocalVariables.ContainsKey(request.Command.AssigningParamOriginalName))
                    request.Command.LocalVariables[request.Command.AssigningParamOriginalName].Value = res;
                else
                {
                    request.Command.LocalVariables[request.Command.AssigningParamOriginalName] =
                        new AgentProperty(commandPair.Item1.OutputArgType, res, request.Command.AssigningParamOriginalName, request.Command.AssigningParameter);
                }
            }
            else
            {
                //На случай вызовов коанд, которые не возвращают значение.
                commandPair.Item2.DynamicInvoke(variables);
            }

            return await Unit.Task;
        }


        private async void ExecuteCommandWithoutCommandCall(ExecuteCodeLineCommand request)
        {
            Regex varRegex = new(@"(?!"")[a-zA-Z]+(?!"")");
            IEnumerable<string> vars = varRegex.Matches(request.Command.OriginCommand
                .Split("=").Last()
                .Trim())
                .Select(x => x.Value.Trim())
                .OrderByDescending(x => x.Length); //Сортировка дял последующей замены от наибольших по длине переменных до наименьших.

            string executableStr = request.Command.OriginCommand.Split("=").Last();

            Type? outputType = null;

            foreach (string var in vars)
            {
                if (request.Command.LocalVariables.ContainsKey(var))
                {
                    if (outputType != null && request.Command.LocalVariables[var].Type != outputType)
                        throw new ExecuteCodeLineException($"Несоответсвие типов переменных в выражении {request.Command.OriginCommand}"); //TODO - Тесты
                    executableStr = executableStr.Replace(var, request.Command.LocalVariables[var].Value.ToString());
                    outputType = request.Command.LocalVariables[var].Type;
                }
                else
                    throw new ExecuteCodeLineException($"Передана переменная {var}, которой не присвоено значение.");
            }

            var scriptState = await CSharpScript.RunAsync(executableStr); //TODO - Тесты
            if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
            {
                if (request.Command.LocalVariables.ContainsKey(request.Command.AssigningParamOriginalName))
                    request.Command.LocalVariables[request.Command.AssigningParamOriginalName].Value = scriptState.ReturnValue;
                else
                    request.Command.LocalVariables[request.Command.AssigningParamOriginalName] = 
                        new AgentProperty(outputType, scriptState.ReturnValue, request.Command.AssigningParamOriginalName, request.Command.AssigningParameter);
            }
        }


        private bool IsContainsCommandCall(string originalCommandName) => originalCommandName.Contains("(") && originalCommandName.Contains(")");
    }
}
