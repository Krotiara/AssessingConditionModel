using Interfaces.Requests;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Entities.Requests;
using PatientsResolver.API.Messaging.Send.Configurations;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Messaging.Send.Sender
{
    public class PatientFileDataSender : IPatientFileDataSender
    {
        private readonly string hostname;
        private readonly string password;
        private readonly string queueName;
        private readonly string username;
        private readonly string exchange;
        private readonly string routingKey;
        private IConnection connection;

        public PatientFileDataSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            queueName = rabbitMqOptions.Value.QueueName;
            hostname = rabbitMqOptions.Value.Hostname;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;
            exchange = rabbitMqOptions.Value.Exchange;
            routingKey = rabbitMqOptions.Value.RoutingKey;
            CreateConnection();
        }

        public bool SendPatientsFileData(IAddInfluencesRequest request)
        {
            try
            {
                if (connection == null)
                    CreateConnection();
                using (IModel channel = connection.CreateModel())
                {
                    QueueDeclareOk status = channel
                        .QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    string jsonString = JsonConvert.SerializeObject(request);
                    byte[] body = Encoding.UTF8.GetBytes(jsonString);
                    channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
                    return true;
                }
            }
            catch(Exception ex)
            {
                //TODO log
                return false;
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
