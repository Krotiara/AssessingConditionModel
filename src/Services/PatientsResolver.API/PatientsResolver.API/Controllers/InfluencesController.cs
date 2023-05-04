using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Entities.Requests;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Query;
using System;

namespace PatientsResolver.API.Controllers
{
    public class InfluencesController : Controller
    {
        private readonly IMediator mediator;

        public InfluencesController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet("patientsApi/influence/{medOrganization}/{influenceId}")]
        public async Task<ActionResult<Influence>> GetPatientInfluence(string medOrganization, int influenceId)
        {
            try
            {
                return await mediator.Send(new GetPatientInfluenceByIdQueue(influenceId, medOrganization));
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


        [HttpDelete("patientsApi/influence/delete/{medOrganization}/{influenceId}")]
        public async Task<ActionResult<bool>> DeletePatientInfluence(string medOrganization, int influenceId) 
        {
            try
            {
                return await mediator.Send(new DeleteInfluenceCommand(influenceId, medOrganization));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #region influences routes
        [HttpPost("patientsApi/influences")]
        public async Task<ActionResult<List<Influence>>> GetPatientInfluences([FromBody]PatientInfluencesRequest request)
        {
            try
            {
                DateTime start = request.StartTimestamp == null? DateTime.MinValue: (DateTime)request.StartTimestamp;
                DateTime end = request.EndTimestamp == null ? DateTime.MaxValue : (DateTime)request.EndTimestamp;

                return Ok(await mediator.Send(new GetPatientInfluencesQuery(request.PatientId, request.MedicalOrganization, start, end)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("patientsApi/influencesWithoutParams")]
        public async Task<ActionResult<List<Influence>>> GetPatientInfluencesWithoutParams([FromBody] PatientInfluencesRequest request)
        {
            try
            {
                DateTime start = request.StartTimestamp == null ? DateTime.MinValue : (DateTime)request.StartTimestamp;
                DateTime end = request.EndTimestamp == null ? DateTime.MaxValue : (DateTime)request.EndTimestamp;

                return Ok(await mediator.Send(new GetPatientInfluencesQuery(request.PatientId, request.MedicalOrganization, start, end, false)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       

        [HttpPost("patientsApi/addInfluenceData/")]
        public async Task<ActionResult<bool>> AddData([FromBody] AddInfluencesRequest request)
        {
            //TODO Добавить статус отсылки
            try
            {
                bool isSuccessSendRequest = await mediator.Send(new SendPatientDataFileSourceCommand() { Request = request });
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
