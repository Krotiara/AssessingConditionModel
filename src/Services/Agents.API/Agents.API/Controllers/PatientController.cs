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
        private readonly ILogger<PatientController> _logger;

        public PatientController(IMediator mediator, IAgentsService agentsService, ILogger<PatientController> logger)
        {
            _mediator = mediator;
            _agentsService = agentsService;
            _logger = logger;
        }


        [HttpPost("agents/init")]
        public async Task<ActionResult> InitAgents([FromBody]InitAgentsRequest request)
        {
            _logger.LogInformation("Init agents");
            foreach (var key in request.AgentsToInit)
                _agentsService.InitAgentBy(key, request.AgentType);
            return Ok();
        }


        [HttpGet("agents/currentState")]
        public async Task<ActionResult> GetAgentCurrentState(AgentKey agentKey) => await PredictState(agentKey, DateTime.Now);
        

        [HttpGet("agents/predict")]
        public async Task<ActionResult> PredictState(AgentKey agentKey, DateTime timeStamp)
        {
            try
            {
                _logger.LogInformation($"Predict state for agent {agentKey.ObservedId} in {agentKey.ObservedObjectAffilation}");
                return Ok(await _mediator.Send(new GetAgentStateQuery(agentKey, timeStamp)));
            }
            catch (GetAgingStateException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
