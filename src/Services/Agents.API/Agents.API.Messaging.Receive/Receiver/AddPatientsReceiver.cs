using Agents.API.Entities;
using Agents.API.Messaging.Receive.Configs;
using Agents.API.Service.Services;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Messaging.Receive.Receiver
{
    public class AddPatientsReceiver : Receiver
    {
        IInitPatientAgentsService initPatientAgentsService;

        readonly IServiceScopeFactory serviceScopeFactory;

        public AddPatientsReceiver(IServiceScopeFactory serviceScopeFactory, IOptions<AddDataConfig> rabbitMqOptions)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            IServiceScope scope = serviceScopeFactory.CreateScope();
            initPatientAgentsService = scope.ServiceProvider.GetRequiredService<IInitPatientAgentsService>();
            //this.initPatientAgentsService = initPatientAgentsService;
            InitReceiver(ReceiveAction, rabbitMqOptions);
        }


        private async Task ReceiveAction(string serializedStr)
        {
            List<IPatient> data = JsonConvert.DeserializeObject<List<Patient>>(serializedStr)
                .Cast<IPatient>()
                .ToList();
            await initPatientAgentsService.InitPatientAgentsAsync(data);
        }
    }
}
