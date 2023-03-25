using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.API.Entities;
using Models.API.Service.Command;
using Models.API.Service.Query;

namespace Models.API.Controllers
{
    [ApiController]
    public class ModelsController : ControllerBase
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
            using (FileStream file = new FileStream(uploadModel.FilePath, FileMode.Open, FileAccess.Read))
                file.CopyTo(ms);
            await _mediator.Send(new InsertModelCommand(uploadModel, ms));
            return Ok();
        }


        [HttpPost("models/{id}")]
        public async Task<IActionResult> GetModelMeta(ModelMetaRequest request)
        {
            ModelMeta meta = await _mediator.Send(new GetModelMetaQuery()
            {
                ModelId = request.Id,
                Version = request.Version
            });
            if (meta == null)
                return NotFound();
            else return Ok(meta);
        }


        [HttpPost("models/predict")]
        public async Task<ActionResult<float[]>> Predict([FromBody]PredictRequest predictRequest)
        {
            ModelMeta meta = await _mediator.Send(new GetModelMetaQuery() { ModelId = predictRequest.ModelId });
            if (meta == null)
                return NotFound();
            float[] output = await _mediator.Send(new PredictModelCommand() { ModelMeta = meta, InputArgs = predictRequest.InputArgs });
            return Ok(output);
        }
    }
}
