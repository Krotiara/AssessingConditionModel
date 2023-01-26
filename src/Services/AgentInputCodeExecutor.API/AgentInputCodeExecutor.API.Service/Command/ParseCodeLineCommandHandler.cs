using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Service.Command
{

    public class ParseCodeLineCommand: IRequest<ICommand>
    {
        public string CodeLine { get; set; }

        public ParseCodeLineCommand(string codeLine)
        {
            CodeLine = codeLine;
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
                return await Task.FromResult((ICommand)new ExecutableCommand(request.CodeLine, CommandType.Assigning, paramName, param));
            }
            else
                return await Task.FromResult(new ExecutableCommand(request.CodeLine, CommandType.VoidCall));
        }
    }
}
