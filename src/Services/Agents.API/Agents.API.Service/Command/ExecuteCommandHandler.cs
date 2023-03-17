using Agents.API.Entities;
using Agents.API.Interfaces;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Command
{

    public class ExecuteCommand: IRequest<object>
    {
        public ExecuteCommand(SystemCommands systemCommand, object[] args)
        {
            SystemCommand = systemCommand;
            this.args = args;
        }

        public SystemCommands SystemCommand { get; set; }

        public object[] args { get; set; }
    }

    public class ExecuteCommandHandler : IRequestHandler<ExecuteCommand, object>
    {
        private readonly ICommandActionsProvider commandActionsProvider;
        private readonly IMediator mediator;

        public ExecuteCommandHandler(ICommandActionsProvider commandActionsProvider, IMediator mediator)
        {
            this.commandActionsProvider = commandActionsProvider;
            this.mediator = mediator;
        }

        public async Task<object> Handle(ExecuteCommand request, CancellationToken cancellationToken)
        {
            Delegate? del = commandActionsProvider.GetDelegateByCommandNameWithoutParams(request.SystemCommand);
            if (del == null)
                throw new ResolveCommandActionException($"Не найден делегат для команды {request.SystemCommand}");

            object[] args = await mediator.Send(new ConvertArgsCommand(request.SystemCommand, request.args));

#warning TODO Нужна мета информация о параметрах - получить по аналогии с GetCommandArgsValuesQueueHandler (сразу преобразованные аргументы).

            object res = del.DynamicInvoke(args);
            if (res is Task)
            {
                await (Task)res;
                res = res.GetType().GetProperty("Result").GetValue(res);
            }
            //if (typeof(T).GetInterface(nameof(IEnumerable<T>)) == null)
            //{
            //    TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
            //    res = typeConverter.ConvertTo(res, typeof(T));
            //}

            return res;
        }
    }
}
