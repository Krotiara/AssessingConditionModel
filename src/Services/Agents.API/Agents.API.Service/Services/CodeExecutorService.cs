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
    public class CodeExecutorService : ICodeExecutor
    {
        private readonly IMediator _mediator;
        private readonly ICodeResolveService _codeResolveService;

        public CodeExecutorService(IMediator mediator, ICodeResolveService codeResolveService)
        {
            this._mediator = mediator;
            _codeResolveService = codeResolveService;
        }


        public async Task<ConcurrentDictionary<string, IProperty>> ExecuteCode(string codeLines,
            IAgent agent,
            IAgentPropertiesNamesSettings commonPropertiesNames,
            CancellationToken cancellationToken = default)
        {
            List<string> lines = codeLines
                .Split("\r\n")
                .Select(x => x.Replace(";", ""))
                .ToList();

            foreach (string codeLine in lines)
            {
                ICommand command = _codeResolveService.ParseCodeLineCommand(codeLine, agent);
                await _mediator.Send(new ExecuteCodeLineCommand(command, commonPropertiesNames), cancellationToken);
            }

            return agent.Variables;
        }
    }
}
