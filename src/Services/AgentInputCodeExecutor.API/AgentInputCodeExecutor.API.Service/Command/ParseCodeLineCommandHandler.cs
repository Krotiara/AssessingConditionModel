﻿using AgentInputCodeExecutor.API.Entities;
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
                bool isParsed = Enum.TryParse("Active", out ParameterNames paramName);
                if (!isParsed)
                    throw new ParseCodeLineException($"Введеный параметр {param} дял присвоения не является допустимым");
                return await Task.FromResult((ICommand)new ExecutableCommand(request.CodeLine, CommandType.Assigning, paramName));
            }
            else
                return await Task.FromResult(new ExecutableCommand(request.CodeLine, CommandType.VoidCall));
        }
    }
}