﻿using Agents.API.Messaging.Receive.Configs;
using Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Messaging.Receive.Receiver
{
    public abstract class Receiver : BackgroundService
    {
        private IModel channel;
        private IConnection connection;

        private  string hostname;
        private  string queueName;
        private  string username;
        private  string password;
        private  string exchange;
        private  string routingKey;

        public async void InitReceiver(Func<string, Task> receiveAction, IOptions<IRabbitMqConfiguration> rabbitMqOptions)
        {
            hostname = rabbitMqOptions.Value.Hostname;
            queueName = rabbitMqOptions.Value.QueueName;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;
            exchange = rabbitMqOptions.Value.Exchange;
            routingKey = rabbitMqOptions.Value.RoutingKey;
            this.receiveAction = receiveAction;
            await InitializeRabbitMqListener();
        }

        private Func<string, Task> receiveAction;

        private Task InitializeRabbitMqListener()
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
                    channel.QueueDeclare(queue: queueName,
                            durable: false, exclusive: false,
                            autoDelete: false, arguments: null);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                //TODO try catch
                return Task.FromException(ex);
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }


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
                await Task.Delay(100, stoppingToken);
                await InitializeRabbitMqListener();
                if(channel == null)
                    throw new Exception("Channel is null and cannot reconnect");
            }
                
            stoppingToken.ThrowIfCancellationRequested();

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (ch, ea) =>
            {
                try
                {
                    string content = Encoding.UTF8.GetString(ea.Body.ToArray());
                    await receiveAction(content);

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Newtonsoft.Json.JsonSerializationException ex)
                {
                    //TODO add log
                    channel.BasicReject(ea.DeliveryTag, true);
                }
                catch(Exception ex)
                {
                    //TODO log
                    channel.BasicReject(ea.DeliveryTag, true);
                }
            };

            channel.BasicConsume(queueName, false, consumer);
        }

    }
}
