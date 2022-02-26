using AssessingConditionModel.Models;
using AssessingConditionModel.Models.DataHandler;
using AssessingConditionModel.Models.PatientModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AssessingConditionModel.Controllers
{
    public class HomeController : Controller
    {
        private readonly PatientsContext patientsDb;

        public HomeController(PatientsContext context)
        {
            patientsDb = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet, Route("patients")]
        public async Task<List<Patient>> GetPatients()
        {
            List<Patient> patients = await patientsDb.Patients
                .Include(p => p.ClinicalParameters)
                    .ThenInclude(с => с.GeneralUrineAnalysis)
                .Include(p => p.ClinicalParameters)
                    .ThenInclude(c => c.GeneralBloodTest)
                .Include(p => p.ClinicalParameters)
                    .ThenInclude(c => c.LungTissueDamage)
                .Include(p => p.FunctionalParameters)
                .Include(p => p.InstrumentalParameters)
                //.AsNoTracking() // https://metanit.com/sharp/entityframework/4.8.php
                .ToListAsync();
            return patients;       
        }


        [HttpGet, Route("patients/{id}")]
        public IActionResult GetPatient(int id)
        {
            Patient p = GetPatientById(id);
            if (p == null)
                RedirectToAction("Index");
            return View("PatientView", p);
        }


        public IActionResult SetParametersTable(int patientId, string parametersIdTable)
        {
            Patient p = GetPatientById(patientId);
            if (p == null)
                RedirectToAction("Index");
            switch (parametersIdTable)
            {
                case "clinicalParameters":
                    return PartialView("PartialParametersView", p.ClinicalParameters);
                case "functionalParameters":
                    return PartialView("PartialParametersView", p.FunctionalParameters);
                case "instrumentalParameters":
                    return PartialView("PartialParametersView", p.InstrumentalParameters);
                default:
                    throw new KeyNotFoundException();
            }
        }


        private Patient GetPatientById(int id)
        {
            return patientsDb.Patients
                .Include(p => p.ClinicalParameters)
                    .ThenInclude(с => с.GeneralUrineAnalysis)
                .Include(p => p.ClinicalParameters)
                    .ThenInclude(c => c.GeneralBloodTest)
                .Include(p => p.ClinicalParameters)
                    .ThenInclude(c => c.LungTissueDamage)
                .Include(p => p.FunctionalParameters)
                .Include(p => p.InstrumentalParameters).SingleOrDefault(s => s.MedicalHistoryNumber.Equals(id));
        }
       
        public async Task<IActionResult> LoadData()
        {
            DataParser dataParser = new DataParser();
            List<Patient> patients = dataParser.LoadData();
            using (var transaction = patientsDb.Database.BeginTransaction())
            {
                try
                {
                    foreach (Patient patient in patients)
                    {
                        if (patientsDb.Patients.Contains(patient))
                        {
                            patientsDb.Update<Patient>(patient);

                        }
                        else
                            patientsDb.Add<Patient>(patient);
                    }
                    await patientsDb.SaveChangesAsync();   
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }
            return RedirectToAction("Index");
        } 


    }
}
