using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class MedicController : Controller
    { 
        public MedicController()
        {
            
        }
        
         public IActionResult Index()
        {
            return View("~/Views/MedicPersonalPage/MedicView.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> AddPatientParameter([Bind("Parameters")] InfluenceViewFormat influence)
        {
            influence.Parameters.Add(new PatientParameter());
            return PartialView("~/Views/Patient/PatientParameterItems.cshtml", influence);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAllPatientParameters([Bind("Parameters")] InfluenceViewFormat influence)
        {
            influence.Parameters.Clear();
            return PartialView("~/Views/Patient/PatientParameterItems.cshtml", influence);
        }
    }
}
