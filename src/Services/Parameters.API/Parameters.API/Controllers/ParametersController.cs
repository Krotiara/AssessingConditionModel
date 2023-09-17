using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parameters.API.Models;
using Parameters.API.Service;

namespace Parameters.API.Controllers
{
    [Route("api/params/[controller]")]
    [ApiController]
    //[Authorize]
    public class ParametersController : ControllerBase
    {
        private readonly ParametersService _paramsService;

        public ParametersController(ParametersService paramsService)
        {
            _paramsService = paramsService;
        }


        [HttpGet("all")]
        public async Task<ActionResult> GetAllParameters()
        {
            var pS = await _paramsService.GetAll();
            return Ok(pS);
        }


        [HttpPost("insert")]
        public async Task<ActionResult> Insert([FromBody] IEnumerable<ACParameter> parameters)
        {
            await _paramsService.Insert(parameters);
            return Ok();
        }


        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] ACParameter parameter)
        {
            await _paramsService.Update(parameter);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _paramsService.Delete(id);
            return Ok();
        }
    }
}
