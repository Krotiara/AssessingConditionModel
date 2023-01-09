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
                return PartialView("~/Views/Patient/SearchPatientView.cshtml");
            else if (tag == "commonBioAgeDynamic")
                return PartialView("~/Views/Patient/CommonAgingDynamicsStartView.cshtml");
            else if (tag == "addPatient")
                return PartialView("~/Views/DataInputPartialViews/AddPatientView.cshtml", new Patient());
            else if (tag == "addInfluence")
                return PartialView("~/Views/DataInputPartialViews/AddInfluenceView.cshtml", new InfluenceViewFormat());
            else if (tag == "addFile")
                return PartialView("~/Views/DataInputPartialViews/AddDataFileView.cshtml");
            else throw new NotImplementedException();
        }
    }
}
