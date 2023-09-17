using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Mongo;
using Agents.API.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agents.API.Controllers
{

    [Route("api/agents/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly SettingsService _settingsService;

        public SettingsController(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }


        [HttpPost("initSettings")]
        public async Task<ActionResult> InitAgentsSettings([FromBody] List<AgentSettings> settings)
        {
            await _settingsService.Insert(settings);
            return Ok();
        }
    }
}
