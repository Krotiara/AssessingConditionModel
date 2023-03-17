using Agents.API.Entities;
using Agents.API.Service.Command;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class CodeExecutorService: ICodeExecutor
    {
        private readonly IMediator _mediator;

        public CodeExecutorService(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task<Dictionary<string, IProperty>> ExecuteCode(string codeLines)
        {
            //TODO нужно возвращать не весь словарь, а только те значения, которые относятся к параметрам агентов.
            List<string> lines = codeLines.Split("\n").ToList();
            Dictionary<string, IProperty> localVars = new Dictionary<string, IProperty>();
            ExecutableAgentCodeSettings settings = new ExecutableAgentCodeSettings(lines, localVars);
            await _mediator.Send(new ExecuteCodeLinesCommand(settings));
            return localVars;
        }

        public async Task<object> ExecuteCommand(string commandName, object[] args)
        {
            bool isValidCommand = Enum.TryParse(commandName, out SystemCommands command);
            if (!isValidCommand)
                throw new NotImplementedException(); //TODO
            object res = await _mediator.Send(new ExecuteCommand(command, args));
            return res;
        }
    }
}
