using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TempGateway.Entities;
using TempGateway.Service.Command;
using TempGateway.Service.Service;

namespace TempGateway.Controllers
{
    public class PatientsController: Controller
    {
        private readonly IMediator mediator;
        private readonly IPatientService patientService;

        public PatientsController(IMediator mediator, IPatientService patientService)
        {
            this.mediator = mediator;
            this.patientService = patientService;
        }


        [HttpGet("patients/{patientId}")]
        public async Task<ActionResult<IPatient>> GetPatient(int patientId)
        {
            IPatient patient = await patientService.GetPatientById(patientId);
            if(patient == null)
                return BadRequest($"No patient found for id {patientId}");
            return Ok(patient);
        }


        [HttpGet("agents/agingState/{patientId}")]
        public async Task<ActionResult<IAgingState>> GetPatientAgingState(int patientId)
        {
            IAgingState agingState = await patientService.GetAgingPatientStateByPatientId(patientId);
            if (agingState == null)
                return BadRequest($"No aging patient state found for id {patientId}");
            return Ok(agingState);
        }


        [HttpPost("agents/agingDynamics/{patientId}")]
        public async Task<ActionResult<IList<IAgingDynamics<AgingState>>>> GetPatientAgingDynamics(int patientId, [FromBody] DateTime[] timeSpan)
        {
            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MaxValue;
            if (timeSpan != null && timeSpan.Length == 2)
            {
                startTime = timeSpan[0];
                endTime = timeSpan[1];
            }
            
            IList<AgingDynamics> agingPatientStates = await patientService.GetAgingDynamicsByPatientId(patientId, startTime, endTime);
            if(agingPatientStates == null)
                return BadRequest($"No aging patient states found for id {patientId}");
            return Ok(agingPatientStates);
        }


        [HttpPost("agents/agingDynamics/")]
        public async Task<ActionResult<IList<IAgingDynamics<AgingState>>>> GetPatientAgingDynamics([FromBody] DateTime[] timeSpan)
        {
            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MaxValue;
            if (timeSpan != null && timeSpan.Length == 2)
            {
                startTime = timeSpan[0];
                endTime = timeSpan[1];
            }
            IList<AgingDynamics> agingPatientStates = await patientService.GetAgingDynamics(startTime, endTime);
            if (agingPatientStates == null)
                return BadRequest($"No aging patient states.");
            return Ok(agingPatientStates);
        }


        [HttpPost("addInfluenceData/")]
        public async Task<ActionResult<bool>> AddInfluenceData([FromBody] FileData fD)
        {
            try
            {
                await mediator.Send(new AddInfluenceDataCommand() { Data = fD });
                return Ok(true);
            }
            catch (AddInfluenceDataException ex)
            {
                return BadRequest($"Add data error:{ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Unexpected error:{ex.Message}");
            }
        }
    }
}
