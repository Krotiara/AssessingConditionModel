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
        [HttpGet("patientsApi/patients/{medOrganization}/{patientId}")]
        public async Task<ActionResult<Patient>> GetPatient(string medOrganization, int patientId)
        {
            Patient patient = await mediator.Send(new GetPatientQuery() { PatientId = patientId, MedicalOrganization = medOrganization });
            if (patient == null)
                return NotFound();
            else 
                return Ok(patient);
           
        }


        [HttpPost("patientsApi/addPatient")]
        public async Task<ActionResult<bool>> AddPatient([FromBody]Patient patient)
        {
            try
            {
                IList<Patient> addedPatients = await mediator.Send(new AddNotExistedPatientsCommand() {Patients = new List<Patient> { patient } });
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
        public async Task<ActionResult<bool>> DeletePatient(int patientId, string medicalOrganization)
        {
            try
            {
                return await mediator.Send(new DeletePatientCommand(patientId, medicalOrganization));
            }
            catch(DeletePatientException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
