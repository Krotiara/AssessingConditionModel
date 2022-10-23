using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TempGateway.Entities;
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
        public async Task<ActionResult<IList<IAgingDynamics<AgingPatientState>>>> GetPatientAgingDynamics(int patientId, [FromBody] DateTime[] timeSpan)
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


        [HttpPost("addInfluenceData/{filePath}")]
        public async Task<ActionResult> AddInfluenceData(string filePath)
        {
            throw new NotImplementedException(); //TODO Преобразовать в массив байт и отослать
        }
    }
}
