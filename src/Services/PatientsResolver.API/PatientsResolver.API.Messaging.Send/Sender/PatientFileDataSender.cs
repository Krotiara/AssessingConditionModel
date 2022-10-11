using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PatientsResolver.API.Entities;
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

        public void SendPatientsFileData(FileData data)
        {
            if (connection == null)
                CreateConnection();
            using (IModel channel = connection.CreateModel())
            {
                //try
                //{
                //    channel.QueueBind(queueName, exchange, routingKey);
                //}
                //catch (RabbitMQ.Client.Exceptions.OperationInterruptedException ex)
                //{
                    QueueDeclareOk status =  channel
                        .QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                //}
                
                string jsonString = JsonConvert.SerializeObject(data);
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
