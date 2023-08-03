using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Mongo;
using Agents.API.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agents.API.Controllers
{
    public class SettingsController : ControllerBase
    {
        private readonly SettingsService _settingsService;

        public SettingsController(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }


        [HttpPost("agents/initSettings")]
        public async Task<ActionResult> InitAgentsSettings(List<AgentSettings> settings)
        {
            await _settingsService.Insert(settings);
            return Ok();
        }
    }
}
