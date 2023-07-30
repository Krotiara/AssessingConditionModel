using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Requests;
using Agents.API.Entities.Response;
using Agents.API.Interfaces;
using Agents.API.Service;
using Agents.API.Service.Services;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agents.API.Controllers
{
    public class AgentsController: ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<AgentsController> _logger;
        private readonly SettingsService _settingsService;
        private readonly AgentsService _agentsService;

        public AgentsController(IMediator mediator, 
            SettingsService settingsService, 
            ILogger<AgentsController> logger, 
            AgentsService agentsService)
        {
            _mediator = mediator;
            _logger = logger;
            _settingsService = settingsService;
            _agentsService = agentsService;
        }


        [HttpPost("agents/predict")]
        public async Task<ActionResult> PredictState([FromBody] PredictionRequest request)
        {
            var sets = _settingsService.Get(request.Affiliation);
            if (sets == null)
                return BadRequest(new { Message = $"No agent settings for {request.Affiliation} affiliation."});        

            PredictionResponse response = new(request.Id, request.Affiliation);
            
            foreach(var predictionSettings in request.Settings)
            {
                IAgentState? state = await _agentsService.GetAgentState(new GetAgentStateRequest()
                {
                    Key = new AgentKey() { ObservedId = request.Id, ObservedObjectAffilation = request.Affiliation },
                    AgentsSettings = sets.Settings,
                    Variables = predictionSettings.Variables
                });

                response.Predictions.Add(new PredictionResponsePart() { Name = predictionSettings.SettingsName, AgentState = state });
            }

            return Ok(response);
        }
    }
}
