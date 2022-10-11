using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerRegister.API.Service.Services
{
    public interface IContainerRegisterService
    {
        public void InitContainer(string imageName);
    }
}
