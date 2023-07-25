using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Entities.Requests;
using PatientsResolver.API.Models.Requests;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Query;
using PatientsResolver.API.Service.Services;
using System;

namespace PatientsResolver.API.Controllers
{
    [Route("patientsApi/[controller]")]
    public class InfluencesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly InfluencesDataService _influencesDataService;

        public InfluencesController(IMediator mediator, InfluencesDataService influencesDataService)
        {
            this.mediator = mediator;
            _influencesDataService = influencesDataService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetInfluenceById(string id)
        {
            throw new NotImplementedException();
        }


        [HttpPost("add")]
        public async Task<ActionResult> AddPatientInfluence([FromBody] Influence influence)
        {
            throw new NotImplementedException();
        }


        [HttpPut("update")]
        public async Task<ActionResult> UpdatePatientInfluence([FromBody] Influence influence)
        {
            throw new NotImplementedException();
        }


        [HttpDelete("delete/{id}")]
        public async Task<AcceptedResult> DeleteInfluence(string id)
        {
            throw new NotImplementedException();
        }


        [HttpPost("influences")]
        public async Task<ActionResult> GetInfluences([FromBody] GetInfluencesRequest request)
        {
            throw new NotImplementedException();
        }


        [HttpPost("addInfluenceData")]
        public async Task<ActionResult> AddData([FromBody] AddInfluencesRequest request)
        {
            //TODO - заменить на сервис
            bool isSuccessSendRequest = await mediator.Send(new SendPatientDataFileSourceCommand() { Request = request });
            return Ok(isSuccessSendRequest);
        }

        //[HttpGet("patientsApi/influence/{medOrganization}/{influenceId}")]
        //public async Task<ActionResult<Influence>> GetPatientInfluence(string medOrganization, int influenceId)
        //{
        //    try
        //    {
        //        return await mediator.Send(new GetPatientInfluenceByIdQueue(influenceId, medOrganization));
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        //[HttpPost("patientsApi/influence/add")]
        //public async Task<ActionResult<bool>> AddPatientInfluence([FromBody] Influence influence)
        //{
        //    try
        //    {
        //        return await mediator.Send(new AddPatientInfluenceCommand(influence));
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        //[HttpPut("patientsApi/influence/update")]
        //public async Task<ActionResult<Influence>> UpdatePatientInfluence([FromBody] Influence influence)
        //{
        //    //TODO тесты
        //    try
        //    {
        //        return await mediator.Send(new UpdateInfluenceCommand(influence));
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        //[HttpDelete("patientsApi/influence/delete/{medOrganization}/{influenceId}")]
        //public async Task<ActionResult<bool>> DeletePatientInfluence(string medOrganization, int influenceId) 
        //{
        //    try
        //    {
        //        return await mediator.Send(new DeleteInfluenceCommand(influenceId, medOrganization));
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        //#region influences routes
        //[HttpPost("patientsApi/influences")]
        //public async Task<ActionResult<List<Influence>>> GetPatientInfluences([FromBody]PatientInfluencesRequest request)
        //{
        //    try
        //    {
        //        DateTime start = request.StartTimestamp == null? DateTime.MinValue: (DateTime)request.StartTimestamp;
        //        DateTime end = request.EndTimestamp == null ? DateTime.MaxValue : (DateTime)request.EndTimestamp;

        //        return Ok(await mediator.Send(new GetPatientInfluencesQuery(request.PatientId, request.MedicalOrganization, start, end)));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("patientsApi/influencesWithoutParams")]
        //public async Task<ActionResult<List<Influence>>> GetPatientInfluencesWithoutParams([FromBody] PatientInfluencesRequest request)
        //{
        //    try
        //    {
        //        DateTime start = request.StartTimestamp == null ? DateTime.MinValue : (DateTime)request.StartTimestamp;
        //        DateTime end = request.EndTimestamp == null ? DateTime.MaxValue : (DateTime)request.EndTimestamp;

        //        return Ok(await mediator.Send(new GetPatientInfluencesQuery(request.PatientId, request.MedicalOrganization, start, end, false)));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}




        //#endregion
    }
}
