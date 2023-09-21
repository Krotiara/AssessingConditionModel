using Interfaces;
using Interfaces.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Documents
{
    public class Influence : Document, IInfluence
    {

        public Influence()
        {

        }

        public string PatientId { get; set; }
        public string Affiliation { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public string InfluenceType { get; set; }
        public string MedicineName { get; set; }
    }
}
