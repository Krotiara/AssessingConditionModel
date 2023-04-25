using Agents.API.Entities;
using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Command
{

    public class ParseCodeLineCommand: IRequest<ICommand>
    {
        public string CodeLine { get; set; }

        public ConcurrentDictionary<string, IProperty> LocalVariables { get; }

        public ConcurrentDictionary<string, IProperty> LocalProperties { get; }

        public ParseCodeLineCommand(string codeLine, 
            ConcurrentDictionary<string, IProperty> localVariables, 
            ConcurrentDictionary<string, IProperty> localProperties)
        {
            CodeLine = codeLine;
            LocalVariables = localVariables;
            LocalProperties = localProperties;
        }
    }

    public class ParseCodeLineCommandHandler : IRequestHandler<ParseCodeLineCommand, ICommand>
    {
        public async Task<ICommand> Handle(ParseCodeLineCommand request, CancellationToken cancellationToken)
        {
            bool isAssigning = request.CodeLine.Contains('=');
            if (isAssigning)
            {
                string param = request.CodeLine.Split('=').First().Trim();
                return new ExecutableCommand(request.CodeLine, CommandType.Assigning, request.LocalVariables, request.LocalProperties, param);
            }
            else
                return new ExecutableCommand(request.CodeLine, CommandType.VoidCall, request.LocalVariables, request.LocalProperties);
        }
    }
}
