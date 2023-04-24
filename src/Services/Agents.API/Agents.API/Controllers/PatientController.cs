using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Requests;
using Agents.API.Interfaces;
using Agents.API.Service.Query;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agents.API.Controllers
{
    public class PatientController: ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IAgentsService _agentsService;
        private readonly ILogger<PatientController> _logger;
        private readonly SettingsStore _settingsStore;

        public PatientController(IMediator mediator, IAgentsService agentsService, SettingsStore settingsStore, ILogger<PatientController> logger)
        {
            _mediator = mediator;
            _agentsService = agentsService;
            _logger = logger;
            _settingsStore = settingsStore;
        }


        //[HttpPost("agents/init")]
        //public async Task<ActionResult> InitAgents([FromBody]InitAgentsRequest request)
        //{
        //    _logger.LogInformation("Init agents");
        //    foreach (var key in request.AgentsToInit)
        //        _agentsService.InitAgentBy(key, request.AgentType);
        //    return Ok();
        //}


        //[HttpGet("agents/currentState")]
        //public async Task<ActionResult> GetAgentCurrentState(AgentKey agentKey) => await PredictState(agentKey, DateTime.Now);
        

        //[HttpGet("agents/predict")]
        //public async Task<ActionResult> PredictState(AgentKey agentKey, DateTime timeStamp)
        //{
        //    try
        //    {
        //        var state = await _mediator.Send(new GetAgentStateQuery(agentKey, timeStamp));
        //        return Ok(state);
        //    }
        //    catch (GetAgingStateException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        [HttpPost("agents/getInfo")]
        public async Task<ActionResult> GetPatientInfo([FromBody]PatientRequest request)
        {
            throw new NotImplementedException();
        }


        [HttpPost("agents/initSettings")]
        public async Task<ActionResult> InitAgentsSettings([FromBody] PredictionModel predictionModel)
        {
            throw new NotImplementedException();
        }
    }
}
