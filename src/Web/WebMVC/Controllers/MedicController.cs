using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

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
            else if (tag == "commonBioAgeDynamic")
                return PartialView("~/Views/Patient/_CommonAgingDynamicsStartView.cshtml");
            else throw new NotImplementedException();
        }


        [HttpGet]
        public async Task<IActionResult> GetDataInputPartialViewByTag(string tag)
        {
            if(tag == "addPatient")
                return PartialView("~/Views/DataInputPartialViews/_AddPatientView.cshtml", new Patient());
            else if (tag == "addInfluence")
                return PartialView("~/Views/DataInputPartialViews/_AddInfluenceView.cshtml", new InfluenceViewFormat());
            else if (tag == "addFile")
                return PartialView("~/Views/DataInputPartialViews/_AddDataFileView.cshtml");
            else throw new NotImplementedException();

        }




    }
}
