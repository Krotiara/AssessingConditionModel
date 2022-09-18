using Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PatientDataHandler.API.Messaging.Send;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API_Messaging.Send.Sender
{
    public class PatientsDataSender : IPatientsDataSender
    {
        private readonly string hostname;
        private readonly string password;
        private readonly string queueName;
        private readonly string username;
        private IConnection connection;

        public PatientsDataSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            queueName = rabbitMqOptions.Value.QueueName;
            hostname = rabbitMqOptions.Value.Hostname;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;

            CreateConnection();
        }

        public void SendPatientsData(IList<IPatientData> data)
        {
            if (connection == null)
                CreateConnection();
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName);

                string json = JsonConvert.SerializeObject(data);
                byte[] body = Encoding.UTF8.GetBytes(json);

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
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }
    }
}
