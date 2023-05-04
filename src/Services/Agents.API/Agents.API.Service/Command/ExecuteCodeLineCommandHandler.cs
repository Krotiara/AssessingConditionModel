using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Interfaces;
using Agents.API.Service.Query;
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

    public class ExecuteCodeLineCommand: IRequest
    {
        public ICommand Command { get; }

        public IAgentPropertiesNamesSettings CommonPropertiesNames { get; }

        public ExecuteCodeLineCommand(ICommand command, IAgentPropertiesNamesSettings commonPropertiesNames)
        {
            Command = command;
            CommonPropertiesNames = commonPropertiesNames;
        }
    }

    public class ExecuteCodeLineCommandHandler : IRequestHandler<ExecuteCodeLineCommand, Unit>
    {
        private readonly ICodeResolveService _codeResolveService;
        private readonly IMediator _mediator;

        public ExecuteCodeLineCommandHandler(ICodeResolveService codeResolveService, IMediator mediator)
        {
            this._codeResolveService = codeResolveService;
            this._mediator = mediator;
        }


        public async Task<Unit> Handle(ExecuteCodeLineCommand request, CancellationToken cancellationToken)
        {
            //Случай простых команд присвоения или расчета без вызова функции.
            if(request.Command.CommandType == CommandType.Assigning && !IsContainsCommandCall(request.Command.OriginCommand))
            {
                await ExecuteCommandWithoutCommandCall(request);
                return await Unit.Task;
            }

#warning Не учтен случай, когда есть и вызов функции, и простые слагаемые. Нужно доьавить обнаружение этого и эксепшн. Усложнять псевдо-выполнитель кода не надо.

            (ICommandArgsTypesMeta, Delegate) commandPair = 
                await _codeResolveService.ResolveCommandAction(request.Command, request.CommonPropertiesNames, cancellationToken);
            List<object> variables = await _mediator.Send(new GetCommandArgsValuesQueue(request.Command, commandPair.Item1), cancellationToken);

            if (request.Command.CommandType == CommandType.Assigning)
            {
                //if sync - return output value, if async - need await
                //https://stackoverflow.com/questions/64766433/c-sharp-asynchronous-invoke-of-generic-delegates
                object res = commandPair.Item2.DynamicInvoke(variables.ToArray());
                if (res is Task)
                {
                    await (Task)res;
                    res = res.GetType().GetProperty("Result").GetValue(res);
                }
                if (commandPair.Item1.OutputArgType.GetInterface(nameof(IEnumerable)) == null)
                {
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(commandPair.Item1.OutputArgType);
                    res = typeConverter.ConvertTo(res, commandPair.Item1.OutputArgType);
                }
               
                if (request.Command.LocalVariables.ContainsKey(request.Command.AssigningParameter))
                    request.Command.LocalVariables[request.Command.AssigningParameter].Value = res;
                else
                {
                    request.Command.LocalVariables[request.Command.AssigningParameter] =
                        new Property()
                        {
                            Name = request.Command.AssigningParameter,
                            Type = commandPair.Item1.OutputArgType,
                            Value = res
                        };
                }
            }
            else
            {
                //На случай вызовов команд, которые не возвращают значение.
                var obj = commandPair.Item2.DynamicInvoke(variables);
                if(obj is Task)
                {
                    //// it's regular Task which does not return the value
                    await (Task)obj;
                }
            }

            return await Unit.Task;
        }


        private async Task ExecuteCommandWithoutCommandCall(ExecuteCodeLineCommand request)
        {
            int index = request.Command.OriginCommand.IndexOf("=");
            string executableStr = request.Command.OriginCommand.Substring(index + 1);
            Regex varRegex = new(@"(?!"")[a-zA-Z]+(?!"")");
            IEnumerable<string> vars = varRegex.Matches(executableStr)
                .Select(x => x.Value.Trim())
                .OrderByDescending(x => x.Length); //Сортировка дял последующей замены от наибольших по длине переменных до наименьших.

            Type? outputType = null;

            foreach (string var in vars)
            {
                IDictionary<string, IProperty> source;
                if (request.Command.LocalVariables.ContainsKey(var))
                    source = request.Command.LocalVariables;
                else if (request.Command.LocalProperties.ContainsKey(var))
                    source = request.Command.LocalProperties;
                else
                    throw new ExecuteCodeLineException($"Передана переменная {var}, которой не присвоено значение.");
                if (outputType != null && source[var].Type != outputType)
                    throw new ExecuteCodeLineException($"Несоответсвие типов переменных в выражении {request.Command.OriginCommand}"); //TODO - Тесты

                executableStr = executableStr.Replace(var, source[var].Value.ToString());
                outputType = source[var].Type;      
            }

            var scriptState = await CSharpScript.RunAsync(executableStr); //TODO - Тесты
            var varsSource = request.Command.LocalVariables;
            if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
            {
                if (varsSource.ContainsKey(request.Command.AssigningParameter))
                    varsSource[request.Command.AssigningParameter].Value = scriptState.ReturnValue;
                else
                    varsSource[request.Command.AssigningParameter] = new Property()
                        {
                            Type = outputType,
                            Name = request.Command.AssigningParameter,
                            Value = scriptState.ReturnValue
                        };
            }
        }


        private bool IsContainsCommandCall(string originalCommandName) => originalCommandName.Contains("(") && originalCommandName.Contains(")");
    }
}
