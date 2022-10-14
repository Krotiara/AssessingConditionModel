using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Messaging.Send.Configurations
{
    public class RabbitMqAddInfoConfig : IRabbitMqConfiguration
    {
        public string Hostname { get ; set ; }
        public string QueueName { get ; set ; }
        public string UserName { get ; set ; }
        public string Password { get ; set ; }
        public bool Enabled { get ; set ; }
        public string Exchange { get ; set ; }
        public string RoutingKey { get ; set ; }
    }
}
