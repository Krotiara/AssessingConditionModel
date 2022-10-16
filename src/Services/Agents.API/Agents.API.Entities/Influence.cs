using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class Influence : IInfluence
    {
        public Influence() { }
        public int Id { get ; set ; }
        public int PatientId { get ; set ; }
        public DateTime StartTimestamp { get ; set ; }
        public DateTime EndTimestamp { get ; set ; }
        public InfluenceTypes InfluenceType { get ; set ; }
        public string MedicineName { get ; set ; }
    }
}
