using Docker.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ContainerRegister.API.Service.Services
{
    internal class DockerClientFactory
    {
        private static readonly Uri defaultWindowsDockerEngineUri = new Uri("npipe://./pipe/docker_engine");
        private static readonly Uri defaultLinuxDockerEngineUri = new Uri("unix:///var/run/docker.sock");

        public static DockerClient CreateInstance()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return new DockerClientConfiguration(defaultWindowsDockerEngineUri).CreateClient();
            else
                return new DockerClientConfiguration(defaultLinuxDockerEngineUri).CreateClient();
        }
    }
}
