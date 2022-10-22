using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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


        [HttpGet("agingState/{patientId}")]
        public async Task<ActionResult<IAgingPatientState>> GetPatientAgingState(int patientId)
        {
            IAgingPatientState agingState = await patientService.GetAgingPatientStateByPatientId(patientId);
            if (agingState == null)
                return BadRequest($"No aging patient state found for id {patientId}");
            return Ok(agingState);
        }


        [HttpPost("agingDynamics/{patientId}")]
        public async Task<ActionResult<IList<IAgingPatientState>>> GetPatientAgingDynamics(int patientId, [FromBody] DateTime[] timeSpan)
        {
            IList<IAgingPatientState> agingPatientStates = await patientService.GetAgingDynamicsByPatientId(patientId);
            if(agingPatientStates == null)
                return BadRequest($"No aging patient states found for id {patientId}");
            return Ok(agingPatientStates);
        }


        [HttpPost("addInfluenceData/{filePath}")]
        public async Task<ActionResult> AddInfluenceData(string filePath)
        {
            throw new NotImplementedException(); //TODO Преобразовать в массив байт и отослать
        }
    }
}
