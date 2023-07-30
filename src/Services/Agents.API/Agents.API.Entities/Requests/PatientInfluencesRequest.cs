using Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests
{
    public class PatientInfluencesRequest : IPatientInfluencesRequest
    {
        public PatientInfluencesRequest() { }

        public string PatientId { get; set; }
        public string Affiliation { get; set; }
        public DateTime? StartTimestamp { get; set; }
        public DateTime? EndTimestamp { get; set; }
    }
}
