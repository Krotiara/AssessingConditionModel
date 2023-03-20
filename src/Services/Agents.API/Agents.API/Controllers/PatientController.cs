using Agents.API.Entities;
using Agents.API.Entities.DynamicAgent;
using Agents.API.Entities.Requests;
using Agents.API.Interfaces;
using Agents.API.Service.Query;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agents.API.Controllers
{
    public class PatientController: Controller
    {

        private readonly IMediator _mediator;
        private readonly IAgentsService _agentsService;

        public PatientController(IMediator mediator, IAgentsService agentsService)
        {
            _mediator = mediator;
            _agentsService = agentsService;
        }


        [HttpPost("agents/init")]
        public async Task<ActionResult> InitAgents([FromBody]InitAgentsRequest request)
        {
            foreach (var pair in request.AgentsToInit)
                _agentsService.InitAgentBy(pair.Item1, pair.Item2);
            return Ok();
        }


        [HttpGet("agents/currentState")]
        public async Task<ActionResult> GetAgentCurrentState(AgentKey agentKey) => await PredictState(agentKey, DateTime.Now);
        

        [HttpGet("agents/predict")]
        public async Task<ActionResult> PredictState(AgentKey agentKey, DateTime timeStamp)
        {
            try
            {
                return Ok(await _mediator.Send(new GetAgentStateQuery(agentKey, timeStamp)));
            }
            catch (GetAgingStateException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
