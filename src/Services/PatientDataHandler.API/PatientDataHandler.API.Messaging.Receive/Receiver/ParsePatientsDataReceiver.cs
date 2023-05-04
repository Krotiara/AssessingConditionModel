﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PatientDataHandler.API.Entities;
using PatientDataHandler.API.Entities.Requests;
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
        private readonly string exchange;
        private readonly string routingKey;

        public ParsePatientsDataReceiver(IParsePatientsDataService parsePatientsDataService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            hostname = rabbitMqOptions.Value.Hostname;
            queueName = rabbitMqOptions.Value.QueueName;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;
            exchange = rabbitMqOptions.Value.Exchange;
            routingKey = rabbitMqOptions.Value.RoutingKey;
            this.parsePatientsDataService = parsePatientsDataService;
            InitializeRabbitMqListener();
        }


        private bool InitializeRabbitMqListener()
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
                connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                channel = connection.CreateModel();
                if (channel != null)
                {
                    channel.QueueDeclare(queue: queueName,
                        durable: false, exclusive: false, autoDelete: false, arguments: null);
                    return true;
                }
                return false;

            }
            catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
            {
                return false;
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
           
        }

        public override void Dispose()
        {
            channel?.Close();
            connection?.Close();
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (channel == null)
            {
                await Task.Delay(500, stoppingToken);
                bool isInit = InitializeRabbitMqListener();
                if (!isInit || channel == null)
                    throw new Exception("PatientDataHandler.API.Messaging.Receive.Receiver channel is null and cannot init");
            }

            stoppingToken.ThrowIfCancellationRequested();
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                try
                {
                    string content = Encoding.UTF8.GetString(ea.Body.ToArray());
                    AddInfluencesRequest fileData = JsonConvert.DeserializeObject<AddInfluencesRequest>(content);
#warning Гарантируется ли, что здесь всегда приходит только дата пациентов, а не все сообщения?
                    //Stream s = GenerateStreamFromString(content);
                    parsePatientsDataService.ParsePatients(fileData);
                    channel?.BasicAck(ea.DeliveryTag, false);
                }
                catch(JsonSerializationException ex)
                {
                    //TODO log
                    channel?.BasicReject(ea.DeliveryTag, false);
                }
            };

            channel?.BasicConsume(queueName, false, consumer);
        }


       
    }
}
