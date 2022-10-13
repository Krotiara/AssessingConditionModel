using Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Messaging.Send.Configurations;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Messaging.Send.Sender
{
    public class PatientsSender : IPatientsSender
    {

        private readonly string hostname;
        private readonly string password;
        private readonly string queueName;
        private readonly string username;
        private readonly string exchange;
        private readonly string routingKey;
        private IConnection connection;

        public PatientsSender(IOptions<RabbitMqUpdateInfoConfig> rabbitMqOptions)
        {
            queueName = rabbitMqOptions.Value.QueueName;
            hostname = rabbitMqOptions.Value.Hostname;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;
            exchange = rabbitMqOptions.Value.Exchange;
            routingKey = rabbitMqOptions.Value.RoutingKey;
            CreateConnection();
        }

        public void SendPatients(List<Patient> patients)
        {
            if (connection == null)
                CreateConnection();
            using (IModel channel = connection.CreateModel())
            {
                QueueDeclareOk status = channel
                    .QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                string jsonString = JsonConvert.SerializeObject(patients);
                byte[] body = Encoding.UTF8.GetBytes(jsonString);
                channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
            }
        }


        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = hostname,
                    UserName = username,
                    Password = password
                };
                connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                //LOG
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }
    }
}
