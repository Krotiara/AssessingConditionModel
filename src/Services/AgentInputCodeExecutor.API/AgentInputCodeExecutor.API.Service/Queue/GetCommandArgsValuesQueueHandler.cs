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
        public GetCommandArgsValuesQueue(string commandLine, ICommandArgsTypesMeta argsMeta, Dictionary<string, object> LocalVariables)
        {
            CommandLine = commandLine;
            CommandArgsTypesMeta = argsMeta;
        }

        public string CommandLine { get; }

        public ICommandArgsTypesMeta CommandArgsTypesMeta { get; }

        public Dictionary<string, object> LocalVariables { get; }
    }

    public class GetCommandArgsValuesQueueHandler : IRequestHandler<GetCommandArgsValuesQueue, List<object>>
    {
        public async Task<List<object>> Handle(GetCommandArgsValuesQueue request, CancellationToken cancellationToken)
        {
#warning нужно тестирование
            Regex argsRegex = new Regex(@"(.*)");
            if (!argsRegex.IsMatch(request.CommandLine))
#warning Может нужно будет прокидывать эксепшн
                return new List<object>();
            List<string> args = argsRegex
                .Match(request.CommandLine).Value
                .Split(',')
                .Select(x => x.Trim())
                .ToList();
            if (args.Count() != request.CommandArgsTypesMeta.InputArgsTypes.Length)
                throw new GetCommandArgsValuesException("Количество переданных аргументов не совпадает с сигнатурой метода");

            List<object> results = new List<object>();
            for(int i =0; i < args.Count();i++)
            {
                if (request.LocalVariables.ContainsKey(args[i]))
                    results.Add(request.LocalVariables[args[i]]);
                else
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(request.CommandArgsTypesMeta.InputArgsTypes[i]);
                    results.Add(converter.ConvertFrom(args[i]));
                }
            }
            return results;
        }
    }
}
