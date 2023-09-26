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

        public SettingsController(SettingsService settingsService)
        {
            _settingsService = settingsService;
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
    }
}
