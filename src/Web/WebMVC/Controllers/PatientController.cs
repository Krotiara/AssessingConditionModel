using AspNetCoreHero.ToastNotification.Abstractions;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class PatientController: Controller
    {
        private readonly IPatientService patientsService;
        private readonly IAgingDynamicsSaveService agingDynamicsSaveService;
        private readonly INotyfService toastNotification;

        public PatientController(IPatientService patientsService, 
            IAgingDynamicsSaveService agingDynamicsSaveService,
            INotyfService toastNotification)
        {
            this.patientsService = patientsService;
            this.agingDynamicsSaveService = agingDynamicsSaveService;
            this.toastNotification = toastNotification;
        }


        [HttpGet]
        public async Task<IActionResult> GetPatientInfo(string id)
        {
            try
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
            catch(GetWebResponceException ex)
            {
                return PartialView("_ErrorPartialView", $"Ошибка получения информации о пациенте с id={id}: {ex.Message}.");
            }  
        }


        [HttpGet]
        public IActionResult AddPatient()
        {
            return PartialView("_AddPatientView", new Patient());
        }


        [HttpPost]
        public async Task<IActionResult> AddPatient(Patient p)
        {
            //TODO 1-может есть более элегантный способ вызвать добавление пациента
            bool isAdd = await patientsService.AddPatient(p);
            if (isAdd)
                toastNotification.Success("Пациент добавлен", 3);
            else
                toastNotification.Error("Не удалось добавить пациента",3);

            return RedirectToAction("Index","Medic");
        }


        [HttpGet("editPatient")]
        public async Task<IActionResult> GetEditPatientView(Patient p)
        {
#warning Нужна корректирока по Html.DisplayFor
            return View("EditPatientView", p);
        }


        [HttpPost]
        public async Task<IActionResult> EditPatient(Patient p)
        {
            bool isEdit = await patientsService.EditPatient(p);

            if (isEdit)
                toastNotification.Success("Данные пациента успешно изменены", 3);
            else
                toastNotification.Error("Не удалось изменить данные пациента", 3);

            return RedirectToAction("Index", "Medic");
        }


        [HttpGet]
        public async Task<IActionResult> GetPatientAgingDynamics(int patientId, DateTime startTimestamp, DateTime endTimestamp)
        {
            try
            {
                IEnumerable<AgingDynamics> agingDynamics = await
                    patientsService.GetPatientAgingDynamics(patientId, startTimestamp, endTimestamp);

                return PartialView("DisplayTemplates/AgingDynamicsCollection", agingDynamics);
            }
            catch(GetWebResponceException ex)
            {
                return PartialView("_ErrorPartialView", $"Ошибка получения динамики биовозраста пациента с id={patientId}: {ex.Message}.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Unexpected error: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetPatientInfluences(int patientId, DateTime startTimestamp, DateTime endTimestamp)
        {
#warning Заменить на DisplayTemplate
            IList<Influence> influences = await patientsService.GetPatientInfluences(patientId, startTimestamp, endTimestamp);
            return PartialView("DisplayTemplates/PatientInfluences", influences);
        }


        [HttpGet("agents/dynamics")]
        public async Task<IActionResult> GetAgingDynamics(DateTime startTimestamp, DateTime endTimestamp)
        {
            try
            {
                List<AgingDynamics> agingDynamics = (await
                   patientsService.GetAgingDynamics(startTimestamp, endTimestamp)).ToList();
                CommonAgingDynamics dynamics = new CommonAgingDynamics(agingDynamics, startTimestamp, endTimestamp);
                return PartialView("DisplayTemplates/CommonAgingDynamics", dynamics);
            }
            catch(GetWebResponceException ex)
            {
                return PartialView("_ErrorPartialView", $"Ошибка получения динамики биовозраста пациентов: {ex.Message}.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Unexpected error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task AddInfluencesFromFile([FromBody]string data)
        {
            //TODO try catch
            byte[] bytes = Convert.FromBase64String(data);
            bool isAdd = await patientsService.AddPatientsInluenceData(bytes);

            if (isAdd)
                toastNotification.Success("Занесение данных пациентов начато успешно", 3);
            else
                toastNotification.Error("Не удалось занести данные пациентов", 3);
        }


        [HttpPost]
        public async Task SaveDynamicsToFile([FromBody]CommonAgingDynamics dynamics)
        {
#warning Проблема получения savePath с серверной части.
            agingDynamicsSaveService.SaveToExcelFile(dynamics);
        }


        [HttpPost]
        public async Task AddInluence(InfluenceViewFormat influenceViewFormat)
        {
            throw new NotImplementedException();
        }
    }
}
