using Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class PatientController: Controller
    {
        private readonly IPatientService patientsService;

        public PatientController(IPatientService patientsService)
        {
            this.patientsService = patientsService;
        }


        [HttpGet]
        public async Task<IActionResult> GetPatientInfo(string id)
        {
            //TODO try catch    
            int patientId = int.Parse(id);
            Patient patient = await patientsService.GetPatient(patientId);
            AgingPatientState state = await patientsService.GetPatientCurrentAgingState(patientId);
            PatientInfo patientInfo = new PatientInfo()
            {
                Patient = patient,
                AgingPatientState = state
            };

            return PartialView("PatientInfoView", patientInfo);         
        }


        [HttpGet]
        public async Task<IActionResult> GetPatientAgingDynamics(int patientId, DateTime startTimestamp, DateTime endTimestamp)
        {
            //TODO try catch
           // int id = int.Parse(patientId);
            IList<AgingDynamics> agingDynamics = await 
                patientsService.GetPatientAgingDynamics(patientId, startTimestamp, endTimestamp);

            return PartialView("PatientAgingDynamicsView", agingDynamics);
        }
    }
}
