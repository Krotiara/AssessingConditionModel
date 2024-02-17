using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Models.Requests;
using PatientsResolver.API.Service.Services;
using System.Linq.Expressions;

namespace PatientsResolver.API.Controllers
{

    [Route("patientsApi/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientsDataService _patientsDataService;


        public PatientsController(PatientsDataService patientsDataService)
        {
            _patientsDataService = patientsDataService;
        }


        [HttpPost("patient")]
        public async Task<ActionResult> GetPatient([FromBody] GetPatientRequest request)
        {
            var patient = await _patientsDataService.Get(request.PatientId, request.Affiliation);
            if (patient == null)
                return Ok();
            return Ok(patient);
        }


        [HttpPost("patients")]
        public async Task<ActionResult> GetPatients([FromBody] GetPatientsRequest r)
        {
            var patients = await _patientsDataService.GetPatients(r.Affiliation, r.Gender, r.InfluenceName, r.StartAge, r.EndAge, r.Start, r.End);
            return Ok(patients);
        }


        [HttpPost("parameters")]
        public async Task<ActionResult> GetPatientParameters([FromBody] GetPatientParametersRequest request)
        {
            DateTime start = request.StartTimestamp == null ? DateTime.MinValue : (DateTime)request.StartTimestamp;
            DateTime end = request.EndTimestamp == null ? DateTime.MaxValue : (DateTime)request.EndTimestamp;
            var parameters = await _patientsDataService.GetPatientParameters(request.PatientId, request.Affiliation, start, end, request.Names);
            return Ok(parameters);
        }


        [HttpPost("addParameters")]
        public async Task<ActionResult> AddPatientParameters([FromBody] AddPatientParametersRequest request)
        {
            foreach(var parameters in request.PatientsParameters)
                await _patientsDataService.AddPatientParameters(parameters.PatientId, parameters.PatientAffiliation, parameters.Parameters);
            return Ok();
        }


        [HttpPost("addPatient")]
        public async Task<ActionResult> AddPatient([FromBody] MongoPatient patient)
        {
            await _patientsDataService.Insert(patient);
            return Ok();
        }


        [HttpPut("updatePatient")]
        public async Task<ActionResult> UpdatePatient([FromBody] MongoPatient patient)
        {
            if (patient.Id == null)
                return Ok();
            await _patientsDataService.Update(patient.Id, patient);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(string id)
        {
            await _patientsDataService.Delete(id);
            return Ok();
        }
    }
}
