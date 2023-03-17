using Agents.API.Entities;
using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Command
{

    public class ParseCodeLineCommand: IRequest<ICommand>
    {
        public string CodeLine { get; set; }

        public Dictionary<string, IProperty> LocalVariables { get; }

        public ParseCodeLineCommand(string codeLine, Dictionary<string, IProperty> localVariables)
        {
            CodeLine = codeLine;
            LocalVariables = localVariables;
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
                bool isParsed = Enum.TryParse(param, out ParameterNames paramName);
                if (!isParsed)
                    paramName = ParameterNames.None; //Сделано для допуска свободных названий переменных.
                   // throw new ParseCodeLineException($"Введеный параметр {param} для присвоения не является допустимым");
                return (ICommand)new ExecutableCommand(request.CodeLine, CommandType.Assigning, request.LocalVariables, param, paramName);
            }
            else
                return new ExecutableCommand(request.CodeLine, CommandType.VoidCall, request.LocalVariables);
        }
    }
}
