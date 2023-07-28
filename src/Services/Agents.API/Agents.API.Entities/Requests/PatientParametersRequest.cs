using Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests
{
    public class PatientParametersRequest
    {
        public PatientParametersRequest(string affiliation, string patientId, DateTime endTimestamp, List<string> names)
        : this(affiliation, patientId, DateTime.MinValue, endTimestamp, names) { }

        public PatientParametersRequest(string affiliation, string patientId, DateTime startTimestamp, DateTime endTimestamp, List<string> names)
        {
            Affiliation = affiliation;
            PatientId = patientId;
            StartTimestamp = startTimestamp;
            EndTimestamp = endTimestamp;
            Names = names;
        }

        public string Affiliation { get; set; }

        public string PatientId { get; set; }

        public DateTime? StartTimestamp { get; set; }

        public DateTime? EndTimestamp { get; set; }

        public List<string> Names { get; set; }
    }
}
