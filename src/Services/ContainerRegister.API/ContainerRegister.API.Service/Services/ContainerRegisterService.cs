using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using TestEnvironment.Docker;

namespace ContainerRegister.API.Service.Services
{
    public class ContainerRegisterService : IContainerRegisterService
    {

        private readonly DockerClient client;
        private static readonly Uri defaultWindowsDockerEngineUri = new Uri("npipe://./pipe/docker_engine");
        private static readonly Uri defaultLinuxDockerEngineUri = new Uri("unix:///var/run/docker.sock");


        public ContainerRegisterService()
        {
            client = CreateInstance();

          
            //client = new DockerClientConfiguration(new Uri("http://host.docker.internal:8001")).CreateClient(); //local
        }

        private DockerClient CreateInstance()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return new DockerClientConfiguration(defaultWindowsDockerEngineUri).CreateClient();
            else
                return new DockerClientConfiguration(defaultLinuxDockerEngineUri).CreateClient();
        }


        public async void InitContainer(string imageName)
        {
#warning Постоянно выскакивает ошибка cannot resolve address или connection refused.
            throw new NotImplementedException();

            //         imageName = "patientdatahandlerapi:dev";
            //         await using var environment = new DockerEnvironmentBuilder()
            //.AddContainer(p => p with
            //{
            //    ImageName = imageName
            //}).Build();

            //         // Up it.
            //         await environment.UpAsync();

            //using (var client = new DockerClientConfiguration(new Uri("http://127.0.0.1:2375")).CreateClient())
            //{
            //    IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
            //     new ContainersListParameters()
            //     {
            //         Limit = 10,
            //     });
            //}




            //CreateContainerResponse response = await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            //{
            //    Name = "test",
            //    Image = imageName
            //});
            //await client.Containers.StartContainerAsync(response.ID, new ContainerStartParameters());
        }
    }
}
