using Interfaces;

namespace PatientDataHandler.API.Messaging.Receive
{
    public class RabbitMqConfiguration: Interfaces.RabbitMqConfiguration
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