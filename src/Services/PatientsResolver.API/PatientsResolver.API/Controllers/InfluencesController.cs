using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Query;

namespace PatientsResolver.API.Controllers
{
    public class InfluencesController : Controller
    {
        private readonly IMediator mediator;

        public InfluencesController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        #region influences routes
        [HttpPost("patientsApi/influence/{patientId}")]
        public async Task<ActionResult<List<Influence>>> GetPatientInfluences(int patientId, [FromBody] DateTime[] timeSpan)
        {
            try
            {
                DateTime start = DateTime.MinValue;
                DateTime end = DateTime.MaxValue;
                if (timeSpan != null && timeSpan.Length == 2)
                {
                    start = timeSpan[0];
                    end = timeSpan[1];
                }

                return Ok(await mediator.Send(new GetPatientInfluencesQuery(patientId, start, end)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("patientsApi/influences/")]
        public async Task<ActionResult<List<Influence>>> GetInfluences([FromBody] DateTime[] timeSpan)
        {
            try
            {
                DateTime start = DateTime.MinValue;
                DateTime end = DateTime.MaxValue;
                if (timeSpan != null && timeSpan.Length == 2)
                {
                    start = timeSpan[0];
                    end = timeSpan[1];
                }
                return Ok(await mediator.Send(new GetInfluencesQuery(start, end)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("patientsApi/addInfluenceData/")]
        public async Task<ActionResult<bool>> AddData([FromBody] FileData fileData)
        {
            //TODO Добавить статус отсылки
            try
            {
                bool isSuccessSendRequest = await mediator.Send(new SendPatientDataFileSourceCommand() { Data = fileData });
                return Ok(isSuccessSendRequest);
            }
            catch (Exception ex)
            {
                //TODO Отлов кастомных ошибок
                return BadRequest(ex.Message);
            }

        }
        #endregion
    }
}
