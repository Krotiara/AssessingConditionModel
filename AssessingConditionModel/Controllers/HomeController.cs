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


       
        public IActionResult LoadData()
        {
            DataParser dataParser = new DataParser();
            List<Patient> patients = dataParser.LoadData();
            //using(var transaction = patientsDb.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        //TODO
            //    }
            //    catch(Exception)
            //    {
            //        transaction.Rollback();
            //    }
            //}
            return RedirectToAction("Index");
        }
    }
}
