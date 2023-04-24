using Agents.API.Entities.AgentsSettings;
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
        public ActionResult InitAgentsSettings([FromBody] PredictionModel predictionModel)
        {
            _settingsService.Insert(predictionModel);
            return Ok();
        }
    }
}
