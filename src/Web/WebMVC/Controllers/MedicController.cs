using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    public class MedicController : Controller
    {
        [HttpGet("data")]
        public async Task<IActionResult> GetPatientsDataView()
        {
#warning Почему-то загружает второй раз при GetPartialViewByTag, поэтому теряются данные
            return View("~/Views/MedicPersonalPage/PatientsDataView.cshtml");
        }


        [HttpGet("dataInput")]
        public async Task<IActionResult> GetDataInputView()
        {
            return View("~/Views/MedicPersonalPage/DataInputView.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> GetPartialViewByTag(string tag)
        {
            if (tag == "search")
                return PartialView("~/Views/Patient/_SearchPatientView.cshtml");
            else if (tag == "add")
                return PartialView("~/Views/Patient/_AddPatientView.cshtml", new Models.Patient());
            else throw new NotImplementedException();
        }
    }
}
