using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    public class MedicController : Controller
    {
        [HttpGet("data")]
        public async Task<IActionResult> GetPatientsDataView() => View("~/Views/MedicPersonalPage/PatientsDataView.cshtml");


        [HttpGet("dataInput")]
        public async Task<IActionResult> GetDataInputView() => View("~/Views/MedicPersonalPage/DataInputView.cshtml");


        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatisticsView() => View("~/Views/MedicPersonalPage/StatisticsView.cshtml");
        

        [HttpGet]
        public async Task<IActionResult> GetPartialViewByTag(string tag)
        {
            if (tag == "search")
                return PartialView("~/Views/Patient/_SearchPatientView.cshtml");
            else if (tag == "add")
                return PartialView("~/Views/Patient/_AddPatientView.cshtml", new Models.Patient());
            else if (tag == "commonBioAgeDynamic")
                return PartialView("~/Views/Patient/_CommonAgingDynamicsStartView.cshtml");
            else throw new NotImplementedException();
        }
    }
}
