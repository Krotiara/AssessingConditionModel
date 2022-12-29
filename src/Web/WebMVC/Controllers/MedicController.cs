using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class MedicController : Controller
    {
        
         public IActionResult Index()
        {
            return View("~/Views/MedicPersonalPage/MedicView.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> GetPartialViewByTag(string tag)
        {
            if (tag == "search") 
                return PartialView("~/Views/Patient/_SearchPatientView.cshtml");
            else if (tag == "commonBioAgeDynamic")
                return PartialView("~/Views/Patient/_CommonAgingDynamicsStartView.cshtml");
            else if (tag == "addPatient")
                return PartialView("~/Views/DataInputPartialViews/_AddPatientView.cshtml", new Patient());
            else if (tag == "addInfluence")
                return PartialView("~/Views/DataInputPartialViews/_AddInfluenceView.cshtml", new InfluenceViewFormat());
            else if (tag == "addFile")
                return PartialView("~/Views/DataInputPartialViews/_AddDataFileView.cshtml");
            else throw new NotImplementedException();
        }


        [HttpPost]
        public async Task<IActionResult> GetPatientInfoPartialView([FromBody]PatientInfoViewSettings InfoViewSettings)
        {
#warning Баг с _PatientInfluencesView - не возвращается. Может из-за скрипта в виде.
            //TODO Вынести GetPartialViewByTag и GetPatientInfoPartialView в отдельный контроллер
            if (InfoViewSettings.Tag == "agingInfo")
                return PartialView("DisplayTemplates/AgingState", InfoViewSettings.PatientInfo.AgingPatientState);
            else if (InfoViewSettings.Tag == "influences")
                return PartialView("~/Views/Patient/_PatientInfluencesView.cshtml");
            else if (InfoViewSettings.Tag == "agingDynamicInfo")
                return PartialView("~/Views/Patient/PatientAgingDynamicsView.cshtml");
            else throw new NotImplementedException();
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
