﻿using Microsoft.Extensions.Hosting;
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

namespace Agents.API.Messaging.Receive.Receiver
{
    public class UpdatePatientsDataReceiver : BackgroundService
    {
        private IModel channel;
        private IConnection connection;
        private readonly IUpdatePatientAgentsService updatePatientAgentsService;
        private readonly string hostname;
        private readonly string queueName;
        private readonly string username;
        private readonly string password;
        private readonly string exchange;
        private readonly string routingKey;

        public UpdatePatientsDataReceiver(IUpdatePatientAgentsService updatePatientAgentsService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            hostname = rabbitMqOptions.Value.Hostname;
            queueName = rabbitMqOptions.Value.QueueName;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;
            exchange = rabbitMqOptions.Value.Exchange;
            routingKey = rabbitMqOptions.Value.RoutingKey;
            this.updatePatientAgentsService = updatePatientAgentsService;
            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
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

                QueueDeclareOk status = channel
                        .QueueDeclare(queue: queueName, 
                        durable: false, exclusive: false, 
                        autoDelete: false, arguments: null);
            }
            catch (Exception ex)
            {
                //TODO try catch

            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            channel?.Close();
            connection?.Close();
            base.Dispose();
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (channel == null)
                return Task.FromException(new Exception("Agents.API.Messaging.Receive.Receiver channel is null"));
            stoppingToken.ThrowIfCancellationRequested();

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (ch, ea) =>
            {
                try
                {
                    string content = Encoding.UTF8.GetString(ea.Body.ToArray());
                    IUpdatePatientsInfo updateInfo = JsonConvert.DeserializeObject<UpdatePatientsInfo>(content);
                    if (updateInfo != null)
                        await updatePatientAgentsService.UpdatePatientAgents(updateInfo.UpdatedIds, new AgentDetermineStateProperties());
                    else
                        throw new NotImplementedException(); //TODO

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Newtonsoft.Json.JsonSerializationException ex)
                {
                    //TODO add log
                    channel.BasicReject(ea.DeliveryTag, false);
                } 
                catch(AgentNotFoundException ex)
                {
                    //TODO add log
                    channel.BasicReject(ea.DeliveryTag, false);
                }
            };

            channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;

        }
    }
}
