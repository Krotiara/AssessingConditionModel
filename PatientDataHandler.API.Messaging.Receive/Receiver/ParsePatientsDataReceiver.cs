using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PatientDataHandler.API.Service.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API.Messaging.Receive.Receiver
{
    public class ParsePatientsDataReceiver : BackgroundService
    {
        private IModel channel;
        private IConnection connection;
        private readonly IParsePatientsDataService parsePatientsDataService;
        private readonly string hostname;
        private readonly string queueName;
        private readonly string username;
        private readonly string password;

        public ParsePatientsDataReceiver(IParsePatientsDataService parsePatientsDataService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            hostname = rabbitMqOptions.Value.Hostname;
            queueName = rabbitMqOptions.Value.QueueName;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;
            this.parsePatientsDataService = parsePatientsDataService;
            InitializeRabbitMqListener();
        }


        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = hostname,
                UserName = username,
                Password = password
            };

            connection = factory.CreateConnection();
            connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
           
        }

        public override void Dispose()
        {
            channel.Close();
            connection.Close();
            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                string content = Encoding.UTF8.GetString(ea.Body.ToArray());
#warning надо ли вообще отjson-вать string?
                var dataFileName = JsonConvert.DeserializeObject<string>(content); 

                parsePatientsDataService.ParsePatients(dataFileName);
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }
    }
}
