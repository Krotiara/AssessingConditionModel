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
            AgingState state = await patientsService.GetPatientCurrentAgingState(patientId);
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
            IList<AgingDynamics> agingDynamics = await 
                patientsService.GetPatientAgingDynamics(patientId, startTimestamp, endTimestamp);

            return PartialView("PatientAgingDynamicsView", agingDynamics);
        }


        [HttpGet]
        public async Task<IActionResult> GetAgingDynamics(DateTime startTimestamp, DateTime endTimestamp)
        {
            //TODO try catch
            IList<AgingDynamics> agingDynamics = await
               patientsService.GetAgingDynamics(startTimestamp, endTimestamp);
            CommonAgingDynamics dynamics = new CommonAgingDynamics(agingDynamics, startTimestamp, endTimestamp);
            return PartialView("CommonAgingDynamicsView", dynamics);
        }


        [HttpPost]
        public async Task AddInfluencesFromFile([FromBody]string data)
        {
            //TODO try catch
            byte[] bytes = Convert.FromBase64String(data);
            _ = await patientsService.AddPatientsInluenceData(bytes);      
        }


        [HttpPost]
        public async Task SaveDynamicsToFile(CommonAgingDynamics agingDynamics)
        {
            throw new NotImplementedException();
        }
    }
}
