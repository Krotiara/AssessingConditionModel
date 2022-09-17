using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Messaging.Receive.Receiver
{
    public class PatientDataUpdateReceiver : BackgroundService
    {
        //private IModel channel;
        //private IConnection connection;
        //private IPatientDataUpdateService patientDataUpdateService;
        //private readonly string hostname;
        //private readonly string queueName;
        //private readonly string username;
        //private readonly string password;

        //public PatientDataUpdateReceiver(IPatientDataUpdateService patientDataUpdateServicem, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        //{

        //}

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
