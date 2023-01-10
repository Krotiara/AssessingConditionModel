using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class ViewGetterController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetPartialViewByTag(string tag)
        {
            if (tag == "search")
                return View("~/Views/Patient/SearchPatientView.cshtml");
            else if (tag == "commonBioAgeDynamic")
                return View("~/Views/Patient/CommonAgingDynamicsStartView.cshtml");
            else if (tag == "addPatient")
                return View("~/Views/DataInputPartialViews/AddPatientView.cshtml", new Patient());
            else if (tag == "addInfluence")
                return View("~/Views/DataInputPartialViews/AddInfluenceView.cshtml", new InfluenceViewFormat());
            else if (tag == "addFile")
                return View("~/Views/DataInputPartialViews/AddDataFileView.cshtml");
            else throw new NotImplementedException();
        }
    }
}
