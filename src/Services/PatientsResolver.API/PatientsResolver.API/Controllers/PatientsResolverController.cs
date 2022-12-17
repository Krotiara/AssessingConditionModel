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

        public PatientsResolverController(IMediator mediator)
        {
            this.mediator = mediator;
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
        public async Task<ActionResult<bool>> AddPatient([FromBody]Patient patient)
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
        public async Task<ActionResult<bool>> UpdatePatient([FromBody]Patient patient)
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
