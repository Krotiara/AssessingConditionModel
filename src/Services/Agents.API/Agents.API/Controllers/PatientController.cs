using Agents.API.Entities;
using Agents.API.Entities.DynamicAgent;
using Agents.API.Service.Query;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agents.API.Controllers
{
    public class PatientController: Controller
    {

        private readonly IMediator mediator;

        public PatientController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpPost("agents/currentState")]
        public async Task<ActionResult> GetAgentCurrentState(AgentKey agentKey) => await PredictState(agentKey, DateTime.Now);
        

        [HttpPost("agents/predict")]
        public async Task<ActionResult> PredictState(AgentKey agentKey, DateTime timeStamp)
        {
            try
            {
                return Ok(await mediator.Send(new GetAgentStateQuery(agentKey, timeStamp)));
            }
            catch (GetAgingStateException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
