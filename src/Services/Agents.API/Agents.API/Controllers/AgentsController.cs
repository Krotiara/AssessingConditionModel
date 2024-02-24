using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Requests;
using Agents.API.Entities.Requests.Responce;
using Agents.API.Interfaces;
using Agents.API.Service;
using Agents.API.Service.Services;
using Interfaces;
using ASMLib.DynamicAgent;
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

        private readonly ILogger<AgentsController> _logger;
        private readonly AgentsService _agentsService;

        public AgentsController(
            ILogger<AgentsController> logger,
            AgentsService agentsService)
        {
            _logger = logger;
            _agentsService = agentsService;
        }


        [HttpPost("predict")]
        public async Task<ActionResult> PredictState([FromBody] PredictionRequest req)
        {
            if (req.AgentSettings == null)
                throw new KeyNotFoundException("Не переданы настройки агентов.");
    
            var key = new AgentKey(req.Id, req.Affiliation, req.AgentType);

            _agentsService.AddPredictionRequest(key, req);

            return Ok();
        }
    }
}
