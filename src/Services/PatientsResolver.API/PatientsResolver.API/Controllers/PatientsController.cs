using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Entities.Requests;
using PatientsResolver.API.Models.Requests;
using PatientsResolver.API.Service.Query;
using PatientsResolver.API.Service.Services;

namespace PatientsResolver.API.Controllers
{

    [Route("patientsApi/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientsDataService _patientsDataService;
        private readonly IMediator _mediator;


        public PatientsController(PatientsDataService patientsDataService, IMediator mediator)
        {
            _patientsDataService = patientsDataService;
            _mediator = mediator;
        }


        [HttpPost("patient")]
        public async Task<ActionResult> GetPatient(GetPatientRequest request)
        {
            throw new NotImplementedException();
        }


        [HttpPost("parameters")]
        public async Task<ActionResult> GetPatientParameters(GetPatientParametersRequest request)
        {
            throw new NotImplementedException();
        }


        [HttpPost("addPatient")]
        public async Task<ActionResult> AddPatient([FromBody] Patient patient)
        {
            throw new NotImplementedException();
        }


        //TODO Изменить Controller на ControllerBase и на возврат ActionResult без указания типа.
        [HttpPut("updatePatient")]
        public async Task<ActionResult> UpdatePatient([FromBody] Patient patient)
        {
            throw new NotImplementedException();
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeletePatient(string id)
        {
            throw new NotImplementedException();
        }


        [HttpPost("latestParameters")]
        public async Task<ActionResult> GetLatestPatientParameters([FromBody] PatientParametersRequest request)
        {
            throw new NotImplementedException();
            //return Ok(await _mediator.Send(new GetLatesPatientParametersQuery(request)));
        }
    }
}
