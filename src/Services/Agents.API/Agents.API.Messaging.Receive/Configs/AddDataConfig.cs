using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Messaging.Receive.Configs
{
    public class AddDataConfig : IRabbitMqConfiguration
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
