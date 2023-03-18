using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientsResolver.API.Entities.Requests;
using PatientsResolver.API.Service.Query;

namespace PatientsResolver.API.Controllers
{
    public class PatientParametersController : Controller
    {
        private readonly IMediator mediator;

        public PatientParametersController(IMediator mediator)
        {
            this.mediator = mediator;
        }



        [HttpPost("patientsApi/latestPatientParameters")]
        public async Task<ActionResult<List<IPatientParameter>>> GetLatestPatientParameters(PatientParametersRequest request)
        {
            try
            {
                return Ok(await mediator.Send(new GetLatesPatientParametersQuery(request)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
