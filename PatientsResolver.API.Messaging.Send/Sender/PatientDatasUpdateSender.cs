using Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Messaging.Send.Sender
{
    public class PatientDatasUpdateSender : IPatientDataUpdateSender
    {
        readonly string hostname;
        readonly string password;
        readonly string queueName;
        readonly string userName;
        private IConnection connection;

        public PatientDatasUpdateSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            queueName = rabbitMqOptions.Value.QueueName;
            hostname = rabbitMqOptions.Value.Hostname;
            password = rabbitMqOptions.Value.Password;
            userName = rabbitMqOptions.Value.UserName;

            CreateConnection();
        }


        public void SendPatientData(IPatientData data)
        {
            if (connection == null)
                CreateConnection();
            using(IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue:queueName);

                string json = JsonConvert.SerializeObject(data);
                byte[] body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange:"", routingKey:queueName,body: body);
            }
        }


        private void CreateConnection()
        {
            try
            {
                ConnectionFactory connectionFactory = new ConnectionFactory
                {
                    HostName = hostname,
                    UserName = userName,
                    Password = password
                };
                connection = connectionFactory.CreateConnection();
            }
            catch(Exception ex)
            {
                //TODO log
            }
        }
    }
}
