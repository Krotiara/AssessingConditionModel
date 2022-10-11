using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using TestEnvironment.Docker;

namespace ContainerRegister.API.Service.Services
{
    public class ContainerRegisterService : IContainerRegisterService
    {
        private readonly DockerClient client;


        public ContainerRegisterService()
        {
            client = DockerClientFactory.CreateInstance();
        }

        //private DockerClient CreateInstance()
        //{
        //    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        //        return new DockerClientConfiguration(defaultWindowsDockerEngineUri).CreateClient();
        //    else
        //        return new DockerClientConfiguration(defaultLinuxDockerEngineUri).CreateClient();
        //}


        public async void InitContainer(string imageName)
        {

            imageName = "influencecalculatorapi:dev";
            string mContainerPort = "5080";
            string mHostPort = "5081";


            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
    new ContainersListParameters()
    {
        Limit = 10,
    });

            CreateContainerResponse response = await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Name = "test",
                Image = imageName,
                Tty = true,
                ExposedPorts = new Dictionary<string, EmptyStruct>() { { mContainerPort, default(EmptyStruct) } },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {{mContainerPort, new List<PortBinding> {new PortBinding {HostPort = mHostPort}}}}
                }
            });
            bool isStarted = await client.Containers.StartContainerAsync(response.ID, new ContainerStartParameters());
        }
    }
}
