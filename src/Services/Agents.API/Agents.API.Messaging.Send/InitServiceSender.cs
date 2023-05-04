using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Messaging.Send
{
    public class InitServiceSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _exchange;
        private readonly string _routingKey;
        private IConnection _connection;
        private readonly ILogger<InitServiceSender> _logger;

        public InitServiceSender(IOptions<InitServiceRabbitConfig> rabbitMqOptions, ILogger<InitServiceSender> logger)
        {
            _queueName = rabbitMqOptions.Value.QueueName;
            _hostname = rabbitMqOptions.Value.Hostname;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _exchange = rabbitMqOptions.Value.Exchange;
            _routingKey = rabbitMqOptions.Value.RoutingKey;
            _logger = logger;
            CreateConnection();
        }


        public async Task Send()
        {
            if (_connection == null)
                CreateConnection();
            using IModel channel = _connection.CreateModel();
            QueueDeclareOk status = channel
                .QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicPublish(exchange: "", routingKey: _queueName);
        }


        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not create connection: {ex.Message}.");
            }
        }
    }
}
