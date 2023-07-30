using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Interfaces;
using Agents.API.Service.AgentCommand;
using Agents.API.Service.Command;
using Amazon.Runtime.Internal;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class CodeExecutorService: ICodeExecutor
    {
        private readonly IMediator _mediator;
        private readonly CommandServiceResolver _commandActionsProvider;

        public CodeExecutorService(IMediator mediator, CommandServiceResolver commandActionsProvider)
        {
            this._mediator = mediator;
            this._commandActionsProvider = commandActionsProvider;
        }


        public async Task<ConcurrentDictionary<string, IProperty>> ExecuteCode(string codeLines,
            ConcurrentDictionary<string, IProperty> variables,
            ConcurrentDictionary<string, IProperty> properties,
            IAgentPropertiesNamesSettings commonPropertiesNames,
            CancellationToken cancellationToken = default)
        {
            List<string> lines = codeLines
                .Split("\r\n")
                .Select(x=>x.Replace(";",""))
                .ToList();
            ConcurrentDictionary<string, IProperty> localVars = new();
            foreach (var pair in variables)
                localVars[pair.Key] = pair.Value;

            ConcurrentDictionary<string, IProperty> localProperties = new();
            foreach (var pair in properties)
                localProperties[pair.Key] = pair.Value;

            foreach (string codeLine in lines)
            {
                ICommand command = await _mediator.Send(new ParseCodeLineCommand(codeLine, localVars, localProperties), cancellationToken);
                await _mediator.Send(new ExecuteCodeLineCommand(command, commonPropertiesNames), cancellationToken);
            }

            return localVars;
        }


        

//        public async Task<object> ExecuteCommand(SystemCommands command, object[] commandArgs, CancellationToken cancellationToken = default)
//        {
//            IAgentCommand c = _commandActionsProvider.Invoke(command);
//            if(c == null)
//                throw new ResolveCommandActionException($"Не найдена команда для {command}");       
//            object[] args = await _mediator.Send(new ConvertArgsCommand(command, commandArgs));

//#warning TODO Нужна мета информация о параметрах - получить по аналогии с GetCommandArgsValuesQueueHandler (сразу преобразованные аргументы).

//            object res = c.Command.DynamicInvoke(args);
//            if (res is Task)
//            {
//                await (Task)res;
//                res = res.GetType().GetProperty("Result").GetValue(res);
//            }
//            //if (typeof(T).GetInterface(nameof(IEnumerable<T>)) == null)
//            //{
//            //    TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
//            //    res = typeConverter.ConvertTo(res, typeof(T));
//            //}

//            return res;
//        }
    }
}
