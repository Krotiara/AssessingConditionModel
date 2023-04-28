using Microsoft.AspNetCore.Mvc;
using Parameters.API.Models.Requests;
using Parameters.API.Services;

namespace Parameters.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametersController : ControllerBase
    {
        private readonly ParametersService _parametersService;

        public ParametersController(ParametersService parametersService)
        {
            _parametersService = parametersService;
        }


        [HttpGet("all")]
        public async Task<ActionResult> GetAll()
        {
            var all = await _parametersService.GetAll();
            return Ok(all);
        }


        [HttpPost("insert")]
        public async Task<ActionResult> Insert(InsertRequest request)
        {
            await _parametersService.Insert(request.Parameters);
            return Ok();
        }
    }
}
