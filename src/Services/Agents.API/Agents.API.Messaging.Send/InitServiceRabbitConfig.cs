using Interfaces;

namespace Agents.API.Messaging.Send
{
    public class InitServiceRabbitConfig: IRabbitMqConfiguration
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