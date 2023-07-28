using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Entities.Requests;
using PatientsResolver.API.Models.Requests;
using PatientsResolver.API.Service.Services;

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
        public async Task<ActionResult> GetPatient(GetPatientRequest request)
        {
            var patient = await _patientsDataService.Get(request.PatientId, request.Affiliation);
            if (patient == null)
                return Ok();
            return Ok(patient);
        }


        [HttpPost("parameters")]
        public async Task<ActionResult> GetPatientParameters(GetPatientParametersRequest request)
        {
            DateTime start = request.StartTimestamp == null ? DateTime.MinValue : (DateTime)request.StartTimestamp;
            DateTime end = request.EndTimestamp == null ? DateTime.MaxValue : (DateTime)request.EndTimestamp;
            var parameters = await _patientsDataService.GetPatientParameters(request.PatientId, request.Affiliation, start, end);
            return Ok(parameters);
        }


        [HttpPost("addPatient")]
        public async Task<ActionResult> AddPatient([FromBody] Patient patient)
        {
            await _patientsDataService.Insert(patient);
            return Ok();
        }


        [HttpPut("updatePatient")]
        public async Task<ActionResult> UpdatePatient([FromBody] Patient patient)
        {
            if (patient.Id == null)
                return Ok();
            await _patientsDataService.Update(patient.Id, patient);
            return Ok();
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeletePatient(string id)
        {
            await _patientsDataService.Delete(id);
            return Ok();
        }
    }
}
