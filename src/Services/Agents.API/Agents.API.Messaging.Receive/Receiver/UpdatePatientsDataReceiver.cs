using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Interfaces;
using Agents.API.Entities;
using Agents.API.Messaging.Receive.Configs;
using Agents.API.Service.Services;

namespace Agents.API.Messaging.Receive.Receiver
{
    public class UpdatePatientsDataReceiver : Receiver
    {
        
        private readonly IUpdatePatientAgentsService updatePatientAgentsService;
       

        public UpdatePatientsDataReceiver(IUpdatePatientAgentsService updatePatientAgentsService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            this.updatePatientAgentsService = updatePatientAgentsService;
            InitReceiver(ReceiveAction, rabbitMqOptions);
        }


        private async Task ReceiveAction(string serializedStr)
        {
            //TODO try catch
            
            IUpdatePatientsDataInfo updateInfo = JsonConvert.DeserializeObject<UpdatePatientsInfo>(serializedStr);
            if (updateInfo != null)
                await updatePatientAgentsService.UpdatePatientAgents(updateInfo);
            else
                throw new NotImplementedException(); //TODO
        }       
    }
}
