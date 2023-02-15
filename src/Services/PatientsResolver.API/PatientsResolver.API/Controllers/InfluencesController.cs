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


        [HttpGet("patientsApi/influence/{influenceId}")]
        public async Task<ActionResult<Influence>> GetPatientInfluence(int influenceId)
        {
            try
            {
                return await mediator.Send(new GetPatientInfluenceByIdQueue(influenceId));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("patientsApi/influence/add")]
        public async Task<ActionResult<bool>> AddPatientInfluence([FromBody] Influence influence)
        {
            try
            {
                return await mediator.Send(new AddPatientInfluenceCommand(influence));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("patientsApi/influence/update")]
        public async Task<ActionResult<Influence>> UpdatePatientInfluence([FromBody] Influence influence)
        {
            //TODO тесты
            try
            {
                return await mediator.Send(new UpdateInfluenceCommand(influence));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("patientsApi/influence/delete/{influenceId}")]
        public async Task<ActionResult<bool>> DeletePatientInfluence(int influenceId) 
        {
            try
            {
                return await mediator.Send(new DeleteInfluenceCommand(influenceId));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #region influences routes
        [HttpPost("patientsApi/influences/{patientId}")]
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

        [HttpPost("patientsApi/influencesWithoutParams/{patientId}")]
        public async Task<ActionResult<List<Influence>>> GetPatientInfluencesWithoutParams(int patientId, [FromBody] DateTime[] timeSpan)
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

                return Ok(await mediator.Send(new GetPatientInfluencesQuery(patientId, start, end, false)));
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
