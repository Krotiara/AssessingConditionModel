using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
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

namespace Agents.API.Service.Command
{

    public class ExecuteCodeLineCommand : IRequest<CommandResult>
    {
        public ICommand Command { get; }

        public IAgentPropertiesNamesSettings CommonPropertiesNames { get; }

        public ExecuteCodeLineCommand(ICommand command, IAgentPropertiesNamesSettings commonPropertiesNames)
        {
            Command = command;
            CommonPropertiesNames = commonPropertiesNames;
        }
    }

    public class ExecuteCodeLineCommandHandler : IRequestHandler<ExecuteCodeLineCommand, CommandResult>
    {
        private readonly ICodeResolveService _codeResolveService;
        private readonly IMediator _mediator;

        public ExecuteCodeLineCommandHandler(ICodeResolveService codeResolveService, IMediator mediator)
        {
            this._codeResolveService = codeResolveService;
            this._mediator = mediator;
        }


        public async Task<CommandResult> Handle(ExecuteCodeLineCommand request, CancellationToken cancellationToken)
        {
            CommandResult res = null;
            //Случай простых команд присвоения или расчета без вызова функции.
            if (request.Command.CommandType == CommandType.Assigning && !IsContainsCommandCall(request.Command.OriginCommand))
            {
                res = await HandleNoCommandExecuting(request);
                return res;
            }

#warning Не учтен случай, когда есть и вызов функции, и простые слагаемые. Нужно доьавить обнаружение этого и эксепшн. Усложнять псевдо-выполнитель кода не надо.

            res = await HandleCommandExecuting(request, cancellationToken);
            return res;
        }


        private async Task<CommandResult> HandleCommandExecuting(ExecuteCodeLineCommand request, CancellationToken cancellationToken)
        {
            (ICommandArgsTypesMeta, Delegate) commandPair =
                await _codeResolveService.ResolveCommandAction(request.Command, request.CommonPropertiesNames, cancellationToken);

            if (commandPair.Item1 == null)
                throw new NotImplementedException(); //TODO

            List<object> variables = _codeResolveService.GetCommandArgsValues(request.Command, commandPair.Item1);

            //if sync - return output value, if async - need await
            //https://stackoverflow.com/questions/64766433/c-sharp-asynchronous-invoke-of-generic-delegates
            object res = commandPair.Item2.DynamicInvoke(variables.ToArray());
            if (res is Task)
            {
                await (Task)res;
                res = res.GetType().GetProperty("Result").GetValue(res);
            }

            CommandResult commandResult = (CommandResult)res; //TODO прокинуть сообщение дальше

            if (request.Command.CommandType == CommandType.Assigning)
            {
                ConvertResult(commandResult, commandPair.Item1.OutputArgType);


                if (request.Command.Agent.Variables.ContainsKey(request.Command.AssigningParameter))
                    request.Command.Agent.Variables[request.Command.AssigningParameter].Value = commandResult.Result;
                else
                    request.Command.Agent.Variables[request.Command.AssigningParameter] =
                        new Property(request.Command.AssigningParameter, commandPair.Item1.OutputArgType.FullName, commandResult.Result);
                
            }

            return commandResult;
        }


        private void ConvertResult(CommandResult res, Type outputType)
        {
            if (outputType.GetInterface(nameof(IEnumerable)) == null)
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(outputType);
                res.Result = typeConverter.ConvertTo(res.Result, outputType);
            }
        }


        private async Task<CommandResult> HandleNoCommandExecuting(ExecuteCodeLineCommand request)
        {
            var variables = request.Command.Agent.Variables;
            var properties = request.Command.Agent.Properties;

            int index = request.Command.OriginCommand.IndexOf("=");
            string executableStr = request.Command.OriginCommand.Substring(index + 1);
            Regex varRegex = new(@"(?!"")[a-zA-Z]+(?!"")");
            IEnumerable<string> vars = varRegex.Matches(executableStr)
                .Select(x => x.Value.Trim())
                .OrderByDescending(x => x.Length); //Сортировка дял последующей замены от наибольших по длине переменных до наименьших.

            string outputType = null;

            foreach (string var in vars)
            {
                IDictionary<string, IProperty> source;
                if (variables.ContainsKey(var))
                    source = variables;
                else if (properties.ContainsKey(var))
                    source = properties;
                else
                    return new CommandResult($"Передана переменная {var}, которой не присвоено значение.");
                if (outputType != null && source[var].Type != outputType)
                    return new CommandResult($"Несоответсвие типов переменных в выражении {request.Command.OriginCommand}");

                executableStr = executableStr.Replace(var, source[var].Value.ToString());
                outputType = source[var].Type;
            }

            var scriptState = await CSharpScript.RunAsync(executableStr); //TODO - Тесты
            var varsSource = variables;
            if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
                return new CommandResult(scriptState.ReturnValue);   
            else
                return new CommandResult("Пустое значение расчета.");
        }


        private bool IsContainsCommandCall(string originalCommandName) => originalCommandName.Contains("(") && originalCommandName.Contains(")");
    }
}
