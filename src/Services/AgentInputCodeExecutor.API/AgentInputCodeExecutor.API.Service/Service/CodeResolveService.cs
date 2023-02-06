using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Command;
using AgentInputCodeExecutor.API.Service.Queue;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Service.Service
{
    public class CodeResolveService : ICodeResolveService
    {

        private readonly IMediator mediator;
        private readonly ICommandActionsProvider commandActionProvider;

        public CodeResolveService(IMediator mediator, ICommandActionsProvider commandActionProvider)
        {
            this.mediator = mediator;
            this.commandActionProvider = commandActionProvider;
        }

        public async Task<(ICommandArgsTypesMeta, Delegate)> ResolveCommandAction(ICommand command, CancellationToken cancellationToken)
        {
            string commandName = await mediator.Send(new GetCommandNameCommand(command), cancellationToken);

            if (commandName == null)
            {
                if (command.CommandType != CommandType.Assigning)
                    throw new ResolveCommandActionException($"Выполнение действия без команды допустимо только для присвоения значения");

                throw new NotImplementedException(); //TODO
            }
            else
            {
                Delegate? del = commandActionProvider.GetDelegateByCommandName(commandName);
                if(del == null)
                    throw new ResolveCommandActionException($"Не удалось разрешить действие для команды {commandName}");
#warning Может вернуться null.
                ICommandArgsTypesMeta meta = await mediator.Send(new GetCommandTypesMetaQueue(commandName), cancellationToken);
                return (meta, del);
            }
        }
    }
}
