using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using Interfaces;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Service.Command
{

    public class ExecuteCodeLineCommand: IRequest
    {
        public ICommand Command { get; }

        public Dictionary<ParameterNames, AgentProperty> Properties { get; }

        public ExecuteCodeLineCommand(ICommand command, Dictionary<ParameterNames, AgentProperty> properties)
        {
            Command = command;
            Properties = properties;
        }
    }

    public class ExecuteCodeLineCommandHandler : IRequestHandler<ExecuteCodeLineCommand, Unit>
    {

        public async Task<Unit> Handle(ExecuteCodeLineCommand request, CancellationToken cancellationToken)
        {
            ScriptState<object> scriptState = await CSharpScript.RunAsync(request.Command.OriginCommand, cancellationToken: cancellationToken);
            if(scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
            {
                if (request.Command.CommandType == CommandType.Assigning)
                    request.Properties[request.Command.AssigningParameter].Value = scriptState.ReturnValue;
            }
            return await Unit.Task;
        }
    }
}
