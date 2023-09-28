using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Documents;
using Agents.API.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agents.API.Controllers
{

    [Route("agentsApi/[controller]")]
    [ApiController]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly SettingsService _settingsService;
        private readonly PredictionRequestsService _predictionRequestsService;

        public SettingsController(SettingsService settingsService, PredictionRequestsService predictionRequestsService)
        {
            _settingsService = settingsService;
            _predictionRequestsService = predictionRequestsService;
        }


        public class InitSettingsRequest
        {
            public List<AgentSettings> Settings { get; set; }
        }


        [HttpPost("initSettings")]
        public async Task<ActionResult> InitAgentsSettings([FromBody] InitSettingsRequest request)
        {
            await _settingsService.Insert(request.Settings);
            return Ok();
        }


        [HttpGet("mlModelsMetas")]
        public ActionResult GetMlModelsMetas() => Ok(_predictionRequestsService.GetMetasDto());
    }
}
