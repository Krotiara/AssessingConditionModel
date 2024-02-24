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
        private readonly PredictionRequestsService _predictionRequestsService;

        public SettingsController(PredictionRequestsService predictionRequestsService)
        {
            _predictionRequestsService = predictionRequestsService;
        }


        [HttpGet("mlModelsMetas")]
        public ActionResult GetMlModelsMetas() => Ok(_predictionRequestsService.GetMetasDto());
    }
}
