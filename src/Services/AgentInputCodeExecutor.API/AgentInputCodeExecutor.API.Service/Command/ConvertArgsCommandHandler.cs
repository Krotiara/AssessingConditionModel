using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Queue;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Service.Command
{
    public class ConvertArgsCommand : IRequest<object[]>
    {

        public ConvertArgsCommand(SystemCommands commandName, object[] rawArgs)
        {
            CommandName = commandName;
            RawArgs = rawArgs;
        }

        public SystemCommands CommandName { get; }

        public object[] RawArgs { get; }
    }

    public class ConvertArgsCommandHandler : IRequestHandler<ConvertArgsCommand, object[]>
    {

        private readonly IMediator mediator;
        public ConvertArgsCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<object[]> Handle(ConvertArgsCommand request, CancellationToken cancellationToken)
        {
            ICommandArgsTypesMeta commandMeta = await mediator.Send(new GetCommandTypesMetaQueue(request.CommandName));
            if (commandMeta == null)
                throw new NotImplementedException();

            if (request.RawArgs.Length != commandMeta.InputArgsTypes.Length)
                throw new ExecuteCommandException("Количество переданных аргументов не соответсвует сигнатуре команды");

            List<object> res = new();

            for(int i = 0; i < request.RawArgs.Length; i++)
            {
                try
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(commandMeta.InputArgsTypes[i]);
                    res.Add(converter.ConvertFrom(request.RawArgs[i]));
                }
                catch (Exception ex)
                {
                    throw new GetCommandArgsValuesException("Ошибка преобразования типа аргумента", ex);
                }
                //if (commandMeta.InputArgsTypes[i] == typeof(string))
                //{
                //    arg = arg.Replace("\"", "");
                //    if (arg == "null")
                //    {
                //        results.Add(null);  //Кривой каст, нужна замена.
                //        continue;
                //    }
                //}
            }

            return res.ToArray();
        }
    }
}
