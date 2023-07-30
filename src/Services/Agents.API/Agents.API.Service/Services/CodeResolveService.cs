﻿using Agents.API.Entities;
using Agents.API.Interfaces;
using Agents.API.Service.AgentCommand;
using Agents.API.Service.Command;
using Agents.API.Service.Query;
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
            string commandName = await _mediator.Send(new GetCommandNameCommand(command), cancellationToken);
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
    }
}
