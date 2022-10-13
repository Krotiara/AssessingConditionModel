using Interfaces;

namespace Agents.API.Messaging.Receive.Configs
{
    public class RabbitMqConfigurationAddData : IRabbitMqConfiguration
    {
        public string Hostname { get; set; }

        public string QueueName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Enabled { get; set; }

        public string Exchange { get; set; }

        public string RoutingKey { get; set; }
    }
}