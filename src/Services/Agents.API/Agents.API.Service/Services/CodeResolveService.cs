using Agents.API.Entities;
using Agents.API.Interfaces;
using Agents.API.Service.AgentCommand;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System.ComponentModel;
using System.Text.RegularExpressions;

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
                IAgentCommand c = _commandActionProvider.Invoke(apiCommand, command.Agent, commonPropertiesNames);
                if (c == null)
                    throw new ResolveCommandActionException($"Не удалось разрешить действие для команды {commandName}");
#warning Может вернуться null.
                ICommandArgsTypesMeta? meta = _metaStorageService.GetMetaByCommandName(apiCommand);
                return (meta, c.Command);
            }
        }


        public List<object> GetCommandArgsValues(ICommand command, ICommandArgsTypesMeta commandArgsTypesMeta)
        {
            var vars = command.Agent.Variables;
            var props = command.Agent.Properties;

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
                if (vars != null && vars.ContainsKey(args[i]))
                    results.Add(vars[args[i]].Value);
                else if (props != null && props.ContainsKey(args[i]))
                    results.Add(props[args[i]].Value);
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


        public ICommand ParseCodeLineCommand(string codeLine, IAgent agent)
        {
            bool isAssigning = codeLine.Contains('=');
            if (isAssigning)
            {
                string param = codeLine.Split('=').First().Trim();
                return new ExecutableCommand(codeLine, CommandType.Assigning, agent, param);
            }
            else
                return new ExecutableCommand(codeLine, CommandType.VoidCall, agent);
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
