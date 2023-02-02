using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<ICodeResolveService, CodeResolveService>();
        }
    }
}
