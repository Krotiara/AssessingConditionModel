using AgentInputCodeExecutor.API.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public JsonResult CalculateAgentParameters([FromBody]ExecutableAgentCodeSettings execCodeSettings)
        {
            throw new NotImplementedException();
        }
    }
}
