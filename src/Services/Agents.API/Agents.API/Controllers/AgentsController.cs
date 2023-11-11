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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agents.API.Controllers
{
    [Route("agentsApi/[controller]")]
    [ApiController]
    [Authorize]
    public class AgentsController : ControllerBase
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


        [HttpPost("predict")]
        public async Task<ActionResult> PredictState([FromBody] PredictionRequest request)
        {
            var sets = await _settingsService.Get(request.Affiliation, request.AgentType);
            if (sets == null)
                return Ok();

            StatePredictions response = new(request.Id, request.Affiliation);

            foreach (var predictionSettings in request.Settings)
            {
                var key = new AgentKey(request.Id, request.Affiliation, request.AgentType);
                StatePrediction p = await _agentsService.GetPrediction(key, sets, predictionSettings);
                if (p != null)
                    response.Predictions.Add(p);
            }

            return Ok(response);
        }
    }
}
