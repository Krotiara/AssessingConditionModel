using ContainerRegister.API.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContainerRegister.API.Controllers
{
    public class ContainerRegisterController : Controller
    {
        private IContainerRegisterService containerRegisterService;

        public ContainerRegisterController(IContainerRegisterService containerRegisterService)
        {
            this.containerRegisterService = containerRegisterService;
        }


        [HttpPost("testInitImage/{imageName}")]
        public ActionResult TestInitImage(string imageName)
        {
            containerRegisterService.InitContainer(imageName);
            return Ok();
        }

    }
}
