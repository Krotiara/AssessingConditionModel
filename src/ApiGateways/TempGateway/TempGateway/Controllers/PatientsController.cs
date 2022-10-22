using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TempGateway.Controllers
{
    public class PatientsController: Controller
    {
        private readonly IMediator mediator;

        public PatientsController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet("patients/{patientId}")]
        public async Task<ActionResult<IPatient>> GetPatient(int patientId)
        {
            throw new NotImplementedException();
        }


        [HttpGet("agingState/{patientId}")]
        public async Task<ActionResult<IAgingPatientState>> GetPatientAgingState(int patientId)
        {
            throw new NotImplementedException();
        }


        [HttpPost("agingDynamics/{patientId}")]
        public async Task<ActionResult<List<IAgingPatientState>>> GetPatientAgingDynamics(int patientId, [FromBody] DateTime[] timeSpan)
        {
            throw new NotImplementedException();
        }


        [HttpPost("addInfluenceData/{filePath}")]
        public async Task<ActionResult> AddInfluenceData(string filePath)
        {
            throw new NotImplementedException(); //TODO Преобразовать в массив байт и отослать
        }
    }
}
