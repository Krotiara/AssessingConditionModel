﻿using Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Messaging.Receive.Receiver
{
    public class AddPatientsDataFromSourceReceiver: BackgroundService
    {
        private IModel channel;
        private IConnection connection;
        private readonly IAddPatientsDataFromSourceService addPatientsDataFromSourceService;
        private readonly string hostname;
        private readonly string queueName;
        private readonly string username;
        private readonly string password;

        public AddPatientsDataFromSourceReceiver(IAddPatientsDataFromSourceService addPatientsDataFromSourceService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
#warning передается пустой RabbitMqConfiguration и происходит краш
            hostname = rabbitMqOptions.Value.Hostname;
            queueName = rabbitMqOptions.Value.QueueName;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;
            this.addPatientsDataFromSourceService = addPatientsDataFromSourceService;
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
                List<PatientData> data = 
                (List<PatientData>)JsonConvert.DeserializeObject<List<IPatientData>>(content)
                .Cast<PatientData>();

                addPatientsDataFromSourceService.AddPatientsData(data);
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }


    }
}