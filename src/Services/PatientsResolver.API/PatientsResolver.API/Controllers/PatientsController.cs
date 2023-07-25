using Microsoft.AspNetCore.Mvc;
using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Models.Requests;
using PatientsResolver.API.Service.Services;

namespace PatientsResolver.API.Controllers
{

    [Route("patientsApi/[controller]")]
    public class PatientsController : Controller
    {
        private readonly PatientsDataService _patientsDataService;


        public PatientsController(PatientsDataService patientsDataService)
        {
            _patientsDataService = patientsDataService;
        }


        [HttpPost("patient")]
        public async Task<ActionResult<Patient>> GetPatient(GetPatientRequest request)
        {
            throw new NotImplementedException();
        }


        [HttpPost("parameters")]
        public async Task<ActionResult<Patient>> GetPatientParameters(GetPatientParametersRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
