using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class CommonAgingDynamics
    {
        public CommonAgingDynamics(IList<AgingDynamics> agingDynamics, 
            DateTime startTimestamp, DateTime endTimestamp)
        {
            StartTimestamp = startTimestamp;
            EndTimestamp = endTimestamp;
            AgingDynamics = agingDynamics;
            
        }

        public CommonAgingDynamics() { }

        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }

        public IList<AgingDynamics> AgingDynamics { get; }
    }
}
