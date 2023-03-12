using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.API.Service.Command;

namespace Models.API.Controllers
{
    public class ModelsController : Controller
    {
        private readonly IMediator _mediator;

        public ModelsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("models/insert")]
        public async Task<IActionResult> InsertModel()
        {
            await _mediator.Send(new InsertModelCommand());
            return Ok();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
