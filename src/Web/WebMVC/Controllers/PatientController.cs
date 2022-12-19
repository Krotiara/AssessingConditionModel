﻿using Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class PatientController: Controller
    {
        private readonly IPatientService patientsService;
        private readonly IAgingDynamicsSaveService agingDynamicsSaveService;

        public PatientController(IPatientService patientsService, 
            IAgingDynamicsSaveService agingDynamicsSaveService)
        {
            this.patientsService = patientsService;
            this.agingDynamicsSaveService = agingDynamicsSaveService;
        }


        [HttpGet]
        public async Task<IActionResult> GetPatientInfo(string id)
        {
            try
            {
                //TODO try catch    
                int patientId = int.Parse(id);
                Patient patient = await patientsService.GetPatient(patientId);
                if (patient == null)
                    throw new Exception("Get patient return null");
                AgingState state = await patientsService.GetPatientCurrentAgingState(patientId);                
                PatientInfo patientInfo = new PatientInfo()
                {
                    Patient = patient,
                    AgingPatientState = state
                };

                return PartialView("_PatientInfoView", patientInfo);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
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
            return RedirectToAction("GetPatientsDataView","Medic");
        }


        [HttpPost]
        public async Task<IActionResult> EditPatient(Patient p)
        {
#warning p все время null (пустой)
            bool isEdit = await patientsService.EditPatient(p);
            return View("Index");
        }


        [HttpGet]
        public async Task<IActionResult> GetPatientAgingDynamics(int patientId, DateTime startTimestamp, DateTime endTimestamp)
        {
            //TODO try catch
            IList<AgingDynamics> agingDynamics = await 
                patientsService.GetPatientAgingDynamics(patientId, startTimestamp, endTimestamp);

            return PartialView("PatientAgingDynamicsView", agingDynamics);
        }


        [HttpGet("agents/dynamics")]
        public async Task<IActionResult> GetAgingDynamics(DateTime startTimestamp, DateTime endTimestamp)
        {
            //TODO try catch
            List<AgingDynamics> agingDynamics = (await
               patientsService.GetAgingDynamics(startTimestamp, endTimestamp)).ToList();
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
        public async Task SaveDynamicsToFile([FromBody]CommonAgingDynamics dynamics)
        {
#warning Проблема получения savePath с серверной части.
            agingDynamicsSaveService.SaveToExcelFile(dynamics);
        }
    }
}
