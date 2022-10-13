using Agents.API.Messaging.Receive.Configs;
using Agents.API.Service.Services;
using Interfaces;
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

        public AddPatientsReceiver(IInitPatientAgentsService initPatientAgentsService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            this.initPatientAgentsService = initPatientAgentsService;
            InitReceiver(ReceiveAction, rabbitMqOptions);
        }


        private void ReceiveAction(string serializedStr)
        {
            List<IPatient> data = JsonConvert.DeserializeObject<List<IPatient>>(serializedStr);
            initPatientAgentsService.InitPatientAgents(data);
        }
    }
}
