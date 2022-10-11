using InfluenceCalculator.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InfluenceCalculator.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}