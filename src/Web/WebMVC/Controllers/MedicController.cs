using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    public class MedicController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetPatientsDataView()
        {
            return View("~/Views/MedicPersonalPage/PatientsDataView.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> GetDataInputView()
        {
            return View("~/Views/MedicPersonalPage/DataInputView.cshtml");
        }
    }
}
