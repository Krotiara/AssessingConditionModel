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
        private readonly IAddInfluencesDataFromSourceService addPatientsDataFromSourceService;
        private readonly string hostname;
        private readonly string queueName;
        private readonly string username;
        private readonly string password;
        private readonly string exchange;
        private readonly string routingKey;

        public AddPatientsDataFromSourceReceiver(IAddInfluencesDataFromSourceService addPatientsDataFromSourceService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            hostname = rabbitMqOptions.Value.Hostname;
            queueName = rabbitMqOptions.Value.QueueName;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;
            exchange = rabbitMqOptions.Value.Exchange;
            routingKey = rabbitMqOptions.Value.RoutingKey;
            this.addPatientsDataFromSourceService = addPatientsDataFromSourceService;
            InitializeRabbitMqListener();
        }

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
                if(channel != null)
                    channel.QueueDeclare(queue: queueName, 
                        durable: false, exclusive: false, autoDelete: false, arguments: null);
                return Task.CompletedTask;
            }
            catch(Exception ex)
            {
                //TODO try catch
                return Task.FromException(ex);
            }


        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e){}

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
                if (channel == null)
                    throw new Exception("PatientsResolver.API.Messaging.Receive.Receiver channel is null and cannot reconnect");
            }

            stoppingToken.ThrowIfCancellationRequested();
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                try
                {
                    string content = Encoding.UTF8.GetString(ea.Body.ToArray());
                    List<Influence> data = JsonConvert.DeserializeObject<List<Influence>>(content);

                    addPatientsDataFromSourceService.AddInfluencesData(data);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Newtonsoft.Json.JsonSerializationException ex)
                {
                    //TODO add log
                    channel.BasicReject(ea.DeliveryTag, false);
                }
            };

            channel.BasicConsume(queueName, false, consumer);
        }


    }
}
