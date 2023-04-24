﻿using Agents.API.Entities;
using Agents.API.Interfaces;
using Agents.API.Service.AgentCommand;
using Agents.API.Service.Command;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class CodeExecutorService: ICodeExecutor
    {
        private readonly IMediator _mediator;
        private readonly CommandServiceResolver _commandActionsProvider;

        public CodeExecutorService(IMediator mediator, CommandServiceResolver commandActionsProvider)
        {
            this._mediator = mediator;
            this._commandActionsProvider = commandActionsProvider;
        }

        public Task<Dictionary<string, IProperty>> ExecuteCode(string codeLines,
            ConcurrentDictionary<string, IProperty> variables, CancellationToken cancellationToken = default)
        {
            //TODO нужно возвращать не весь словарь, а только те значения, которые относятся к параметрам агентов.
            List<string> lines = codeLines.Split("\n").ToList();
            Dictionary<string, IProperty> localVars = new Dictionary<string, IProperty>();
            ExecutableAgentCodeSettings settings = new ExecutableAgentCodeSettings(lines, localVars);
            Dictionary<string, IProperty>  LocalVariables = settings.Properties; //TODO Сейчас прокидывается через settings. По идее можно убрать прокидывание и оставить только здесь инициализацию.
            foreach (string codeLine in settings.CodeLines)
            {
                ICommand command = await _mediator.Send(new ParseCodeLineCommand(codeLine, LocalVariables), cancellationToken);
                await _mediator.Send(new ExecuteCodeLineCommand(command), cancellationToken);
            }

            return localVars;
        }


        public async Task<object> ExecuteCommand(SystemCommands command, object[] commandArgs, CancellationToken cancellationToken = default)
        {
            IAgentCommand c = _commandActionsProvider.Invoke(command);
            if(c == null)
                throw new ResolveCommandActionException($"Не найдена команда для {command}");       
            object[] args = await _mediator.Send(new ConvertArgsCommand(command, commandArgs));

#warning TODO Нужна мета информация о параметрах - получить по аналогии с GetCommandArgsValuesQueueHandler (сразу преобразованные аргументы).

            object res = c.Command.DynamicInvoke(args);
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
