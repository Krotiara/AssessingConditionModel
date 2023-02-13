﻿using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Service.Command;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InputCodeMatcher.API.Controllers
{
    public class CodeExecutorController : Controller
    {
        private readonly IMediator mediator;

        public CodeExecutorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        
        [HttpPost("codeExecutor/executeCode")]
        public async Task<ContentResult> CalculateAgentParameters([FromBody]string codeLines)
        {
            //TODO нужно возвращать не весь словарь, а только те значения, которые относятся к параметрам агентов.
            List<string> lines = codeLines.Split("\n").ToList();
            Dictionary<string, IProperty> localVars = new Dictionary<string, IProperty>();
            ExecutableAgentCodeSettings settings = new ExecutableAgentCodeSettings(lines, localVars);
            await mediator.Send(new ExecuteCodeLinesCommand(settings));
            string a = JsonConvert.SerializeObject(localVars);
            return Content(JsonConvert.SerializeObject(localVars), "application/json");
        }


        [HttpPost("codeExecutor/executeCommand/{commandName}")]
        public async Task<ContentResult> ExecuteCommand(string commandName, [FromBody] object[] args)
        {
            bool isValidCommand = Enum.TryParse(commandName, out SystemCommands command);
            if (!isValidCommand)
                throw new NotImplementedException(); //TODO
            object res = await mediator.Send(new ExecuteCommand(command, args));
            return Content(JsonConvert.SerializeObject(res), "application/json");
        }
    }
}
