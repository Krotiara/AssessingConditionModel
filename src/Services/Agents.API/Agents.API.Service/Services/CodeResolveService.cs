using Agents.API.Entities;
using Agents.API.Entities.Requests;
using Agents.API.Interfaces;
using Agents.API.Service.AgentCommand;
using Agents.API.Service.Command;
using Amazon.Runtime.Internal;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class CodeResolveService : ICodeResolveService
    {

        private readonly IMediator _mediator;
        private readonly CommandServiceResolver _commandActionProvider;
        private readonly IMetaStorageService _metaStorageService;

        public CodeResolveService(IMediator mediator,
            CommandServiceResolver commandActionProvider, IMetaStorageService metaStorageService)
        {
            this._mediator = mediator;
            this._commandActionProvider = commandActionProvider;
            _metaStorageService = metaStorageService;
        }

        public async Task<(ICommandArgsTypesMeta?, Delegate)> ResolveCommandAction(ICommand command,
            IAgentPropertiesNamesSettings commonPropertiesNames, CancellationToken cancellationToken)
        {
            string commandName = GetCommandName(command);
            SystemCommands apiCommand;
            try
            {
                apiCommand = (SystemCommands)Enum.Parse(typeof(SystemCommands), commandName);
            }
            catch (ArgumentException)
            {
                throw new ResolveCommandActionException($"Была передана команда, не содержащаяся в API системы: {commandName}");
            }

            if (commandName == null)
            {
                if (command.CommandType != CommandType.Assigning)
                    throw new ResolveCommandActionException($"Выполнение действия без команды допустимо только для присвоения значения");

                throw new NotImplementedException(); //TODO
            }
            else
            {
#warning В тесте не вызывается метод, а взовращается делегат метода-теста. WAT
                IAgentCommand c = _commandActionProvider.Invoke(apiCommand, command.LocalVariables, command.LocalProperties, commonPropertiesNames);
                if (c == null)
                    throw new ResolveCommandActionException($"Не удалось разрешить действие для команды {commandName}");
#warning Может вернуться null.
                ICommandArgsTypesMeta? meta = _metaStorageService.GetMetaByCommandName(apiCommand);
                return (meta, c.Command);
            }
        }


        public List<object> GetCommandArgsValues(ICommand command, ICommandArgsTypesMeta commandArgsTypesMeta)
        {
#warning нужно тестирование
            Regex argsRegex = new Regex(@"\(.*\)");
            if (!argsRegex.IsMatch(command.OriginCommand))
#warning Может нужно будет прокидывать эксепшн
                return new List<object>();
            List<string> args = argsRegex
                .Match(command.OriginCommand).Value
                .Replace("(", "")
                .Replace(")", "")
                .Split(',')
            .Select(x => x.Trim())
            .Where(x => x != string.Empty)
                .ToList();
            if (args.Count() != commandArgsTypesMeta.InputArgsTypes.Length)
                throw new GetCommandArgsValuesException("Количество переданных аргументов не совпадает с сигнатурой метода");

            List<object> results = new List<object>();
            for (int i = 0; i < args.Count(); i++)
            {
                if (command.LocalVariables != null && command.LocalVariables.ContainsKey(args[i]))
                    results.Add(command.LocalVariables[args[i]].Value);
                else if (command.LocalProperties != null && command.LocalProperties.ContainsKey(args[i]))
                    results.Add(command.LocalProperties[args[i]].Value);
                else
                {
                    string arg = args[i];
                    if (commandArgsTypesMeta.InputArgsTypes[i] == typeof(string))
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
                        TypeConverter converter = TypeDescriptor.GetConverter(commandArgsTypesMeta.InputArgsTypes[i]);
                        results.Add(converter.ConvertFrom(arg));
                    }
                    catch (Exception ex)
                    {
                        throw new GetCommandArgsValuesException("Ошибка преобразования типа аргумента", ex);
                    }
                }
            }
            return results;
        }


        public ICommand ParseCodeLineCommand(ParseCodeLineRequest request)
        {
            bool isAssigning = request.CodeLine.Contains('=');
            if (isAssigning)
            {
                string param = request.CodeLine.Split('=').First().Trim();
                return new ExecutableCommand(request.CodeLine, CommandType.Assigning, request.LocalVariables, request.LocalProperties, param);
            }
            else
                return new ExecutableCommand(request.CodeLine, CommandType.VoidCall, request.LocalVariables, request.LocalProperties);
        }


        public string GetCommandName(ICommand command)
        {
            Regex methodCallRegex = new Regex(@"=.+\(.*\)");
            Match match = methodCallRegex.Match(command.OriginCommand);
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
