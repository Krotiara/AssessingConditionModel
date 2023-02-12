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


        [HttpGet("agents/agingState/{patientId}")]
        public async Task<ActionResult<IAgingState>> GetAgingState(int patientId)
        {
            try
            {
                return await mediator.Send(new GetAgingStateQuery() { PatientId = patientId });
            }
            catch(GetAgingStateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("agents/agingDynamics/{patientId}")]
        public async Task<ActionResult<IList<IAgingDynamics<AgingState>>>> GetAgingDynamics(int patientId, [FromBody]DateTime[] timeSpan)
        {
            try
            {
                DateTime start = DateTime.MinValue;
                DateTime end = DateTime.MaxValue;
                if(timeSpan != null && timeSpan.Length == 2)
                {
                    start = timeSpan[0];
                    end = timeSpan[1];
                }
                return await mediator.Send(new GetAgingDynamicsQuery() { PatientId = patientId, StartTimestamp = start, EndTimestamp = end });
            }
            catch(GetAgingDynamicsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest($"Unexpected error: {ex}");
            }
        }


        [HttpPost("agents/agingDynamics/")]
        public async Task<ActionResult<IList<IAgingDynamics<AgingState>>>> GetAgingDynamics([FromBody] DateTime[] timeSpan)
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
                return await mediator.Send(new GetAllPatientsAgingDynamicsQuery() {StartTimestamp = start, EndTimestamp = end });
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();//TODO
            }
        }
    }
}
