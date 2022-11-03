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
using Microsoft.Extensions.DependencyInjection;

namespace Agents.API.Messaging.Receive.Receiver
{
    public class UpdatePatientsDataReceiver : Receiver
    {
        
        private readonly IUpdatePatientAgentsService updatePatientAgentsService;
        readonly IServiceScopeFactory serviceScopeFactory;

        public UpdatePatientsDataReceiver(IServiceScopeFactory serviceScopeFactory, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            IServiceScope scope = serviceScopeFactory.CreateScope();
            updatePatientAgentsService = scope.ServiceProvider.GetRequiredService<IUpdatePatientAgentsService>();
           // this.updatePatientAgentsService = updatePatientAgentsService;
            InitReceiver(ReceiveAction, rabbitMqOptions);
        }


        private async Task ReceiveAction(string serializedStr)
        {
            //TODO try catch
            try
            {
                IUpdatePatientsDataInfo updateInfo = JsonConvert.DeserializeObject<UpdatePatientsInfo>(serializedStr);
                if (updateInfo != null)
                    await updatePatientAgentsService.UpdatePatientAgents(updateInfo);
                else
                    throw new NotImplementedException(); //TODO
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
                //TODO
            }
        }       
    }
}
