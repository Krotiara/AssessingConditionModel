using Agents.API.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Agents.API.Service.Command
{

    /// <summary>
    /// Возвращает null, если строка команды не содержит в себе вызова функции.
    /// </summary>
    public class GetCommandNameCommand : IRequest<string>
    {
        public ICommand Command { get; }

        public GetCommandNameCommand(ICommand command)
        {
            Command = command;
        }
    }


    public class GetCommandNameCommandHandler : IRequestHandler<GetCommandNameCommand, string>
    {
        public async Task<string> Handle(GetCommandNameCommand request, CancellationToken cancellationToken)
        {
            Regex methodCallRegex = new Regex(@"=.+\(.*\)");
            Match match = methodCallRegex.Match(request.Command.OriginCommand);
            if (match.Success)
            {
                string commandName = match
                    .Value
                    .Replace("=", "")
                    .Trim()
                    .Split('(')
                    .First();
                return commandName;
            }
            else
                return null;
        }
    }
}
