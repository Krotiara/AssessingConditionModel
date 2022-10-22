using Agents.API.Data.Database;
using Agents.API.Entities;
using Agents.API.Service.Query;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agents.API.Controllers
{
    public class PatientController: Controller
    {

        private readonly IMediator mediator;

        public PatientController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet("agingState/{patientId}")]
        public async Task<ActionResult<IAgingPatientState>> GetAgingState(int patientId)
        {
            try
            {
                return await mediator.Send(new GetAgingStateQuery() { PatientId = patientId });
            }
            catch(GetAgingStateException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("agingDynamics/{patientId}")]
        public async Task<ActionResult<IList<IAgingPatientState>>> GetAgingDynamics(int patientId, [FromBody] DateTime[] timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
