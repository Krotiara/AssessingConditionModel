using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Requests;
using Agents.API.Entities.Requests.Responce;
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
        public async Task<ActionResult> PredictState([FromBody] PredictionRequest req)
        {
            var sets = await _settingsService.Get(req.Affiliation, req.AgentType);
            if (sets == null)
                return Ok();

            List<StatePrediction> predictions = new();
            foreach (var predictionSettings in req.Settings)
            {
                var key = new AgentKey(req.Id, req.Affiliation, req.AgentType);
                StatePredictionResponce p = await _agentsService.GetPrediction(key, sets, predictionSettings);
                if (p.IsError)
                    return Ok(new StatePredictionsResponce(req.Id, req.Affiliation) { ErrorMessage = p.ErrorMessage });

                predictions.Add(p.StatePrediction);
            }

            return Ok(new StatePredictionsResponce(req.Id, req.Affiliation, predictions));
        }
    }
}
