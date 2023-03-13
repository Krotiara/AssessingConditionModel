using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.API.Entities;
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
        public async Task<IActionResult> InsertModel([FromForm] UploadModel uploadModel)
        {
            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(uploadModel.File, FileMode.Open, FileAccess.Read))
                file.CopyTo(ms);
            await _mediator.Send(new InsertModelCommand(uploadModel, ms));
            return Ok();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
