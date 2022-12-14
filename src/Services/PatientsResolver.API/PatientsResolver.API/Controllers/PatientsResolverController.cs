using AutoMapper;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Exceptions;
using PatientsResolver.API.Service.Query;
using System.Text;

namespace PatientsResolver.API.Controllers
{
    public class PatientsResolverController : Controller
    {
        private readonly IMediator mediator;
        //private readonly IMapper mapper;

        public PatientsResolverController(IMediator mediator)
        {
            //this.mapper = mapper;
            this.mediator = mediator;
        }


        [HttpPost("patientsApi/latestPatientParameters/{patientId}")]
        public async Task<ActionResult<List<IPatientParameter>>> GetLatestPatientParameters(int patientId, [FromBody]DateTime[] timeSpan)
        {
            try
            {  
                return Ok(await mediator.Send(new GetLatesPatientParametersQuery() { PatientId = patientId, 
                    StartTimestamp = timeSpan.FirstOrDefault(), 
                    EndTimestamp = timeSpan.LastOrDefault()}));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("patientsApi/influence/{patientId}")]
        public async Task<ActionResult<List<Influence>>> GetPatientInfluences(int patientId, [FromBody]DateTime[] timeSpan)
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
            catch(Exception ex)
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
            catch(Exception ex)
            {
                //TODO Отлов кастомных ошибок
                return BadRequest(ex.Message);
            }

        }


        #region patients routes
        [HttpGet("patientsApi/patients/{patientId}")]
        public async Task<ActionResult<Patient>> GetPatient(int patientId)
        {
            try
            {
                return Ok(await mediator.Send(new GetPatientQuery() { PatientId = patientId }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("patientsApi/addPatient")]
        public async Task<ActionResult<bool>> AddPatient(Patient patient)
        {
            try
            {
                IList<Patient> addedPatients = await mediator.Send(new AddNotExistedPatientsCommand() {Patients = new List<Patient> { patient } });
                if (addedPatients.Count > 0)
                    await mediator.Send(new SendPatientsCommand() { Patients = addedPatients.ToList() });

                return Ok(addedPatients.Count > 0);
            }
            catch(AddPatientException ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpPut("patientsApi/updatePatient")]
        public async Task<ActionResult<bool>> UpdatePatient(Patient patient)
        {
            try
            {
                Patient upd = await mediator.Send(new UpdatePatientCommand(patient));
                //await mediator.Send(new SendPatientsCommand() { Patients = new List<Patient>() { upd } });
                return true; //Криво
            }
            catch(UpdatePatientException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("patientsApi/deletePatient")]
        public async Task<ActionResult<bool>> DeletePatient(int patientId)
        {
            try
            {
                return await mediator.Send(new DeletePatientCommand(patientId));
            }
            catch(DeletePatientException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
