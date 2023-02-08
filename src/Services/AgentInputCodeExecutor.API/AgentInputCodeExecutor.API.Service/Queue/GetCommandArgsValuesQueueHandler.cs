using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Service.Queue
{

    public class GetCommandArgsValuesQueue : IRequest<List<object>>
    {
        public GetCommandArgsValuesQueue(ICommand command, ICommandArgsTypesMeta argsMeta)
        {
            Command = command;
            CommandArgsTypesMeta = argsMeta;

        }

        public ICommand Command { get; }

        public ICommandArgsTypesMeta CommandArgsTypesMeta { get; }
    }

    public class GetCommandArgsValuesQueueHandler : IRequestHandler<GetCommandArgsValuesQueue, List<object>>
    {
        public async Task<List<object>> Handle(GetCommandArgsValuesQueue request, CancellationToken cancellationToken)
        {
#warning нужно тестирование
            Regex argsRegex = new Regex(@"\(.*\)");
            if (!argsRegex.IsMatch(request.Command.OriginCommand))
#warning Может нужно будет прокидывать эксепшн
                return new List<object>();
            List<string> args = argsRegex
                .Match(request.Command.OriginCommand).Value
                .Replace("(","")
                .Replace(")","")
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => x != string.Empty)
                .ToList();
            if (args.Count() != request.CommandArgsTypesMeta.InputArgsTypes.Length)
                throw new GetCommandArgsValuesException("Количество переданных аргументов не совпадает с сигнатурой метода");

            List<object> results = new List<object>();
            for(int i =0; i < args.Count();i++)
            {
                if (request.Command.LocalVariables != null && request.Command.LocalVariables.ContainsKey(args[i]))
                    results.Add(request.Command.LocalVariables[args[i]].Value);
                else
                {
                    string arg = args[i];
                    if (request.CommandArgsTypesMeta.InputArgsTypes[i] == typeof(string))
                    {
                        arg = arg.Replace("\"", "");
                        if (arg == "null")
                        {
                            results.Add(null);  //Кривой каст, нужна замена.
                            continue;
                        }
                    }
                    try
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(request.CommandArgsTypesMeta.InputArgsTypes[i]);
                        results.Add(converter.ConvertFrom(arg));
                    }
                    catch(Exception ex)
                    {
                        throw new GetCommandArgsValuesException("Ошибка преобразования типа аргумента", ex);
                    }
                }
            }
            return results;
        }
    }
}
