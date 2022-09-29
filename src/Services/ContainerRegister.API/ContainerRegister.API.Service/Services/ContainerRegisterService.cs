using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace ContainerRegister.API.Service.Services
{
    public class ContainerRegisterService : IContainerRegisterService
    {

        private readonly DockerClient client;

        public ContainerRegisterService()
        {
            client = new DockerClientConfiguration().CreateClient(); //local
        }


        public async void InitContainer(string imageName)
        {
            CreateContainerResponse response = await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = imageName
            });
            await client.Containers.StartContainerAsync(response.ID, new ContainerStartParameters());
        }
    }
}
