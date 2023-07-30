using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Requests;
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
        private readonly CodeResolveService _codeResolveService;

        public CodeExecutorService(IMediator mediator, CommandServiceResolver commandActionsProvider, CodeResolveService codeResolveService)
        {
            this._mediator = mediator;
            this._commandActionsProvider = commandActionsProvider;
            _codeResolveService = codeResolveService;
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
                ICommand command = _codeResolveService.ParseCodeLineCommand(new ParseCodeLineRequest(codeLine, localVars, localProperties));
                await _mediator.Send(new ExecuteCodeLineCommand(command, commonPropertiesNames), cancellationToken);
            }

            return localVars;
        }
    }
}
