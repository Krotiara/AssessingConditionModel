using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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



        [HttpPost("patientsApi/latestPatientParameters/{patientId}")]
        public async Task<ActionResult<List<IPatientParameter>>> GetLatestPatientParameters(int patientId, [FromBody] DateTime[] timeSpan)
        {
            try
            {
                return Ok(await mediator.Send(new GetLatesPatientParametersQuery()
                {
                    PatientId = patientId,
                    StartTimestamp = timeSpan.FirstOrDefault(),
                    EndTimestamp = timeSpan.LastOrDefault()
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
