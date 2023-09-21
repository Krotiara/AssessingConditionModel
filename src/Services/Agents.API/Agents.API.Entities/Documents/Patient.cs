using Interfaces;
using Interfaces.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Agents.API.Entities.Documents
{
    public class Patient : Document, IPatient
    {
        public Patient()
        {
            Parameters = new();
        }

        public string PatientId { get; set; }
        public string Affiliation { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public GenderEnum Gender { get; set; }
        public TreatmentStatus TreatmentStatus { get; set; }

        [JsonIgnore]
        public ConcurrentDictionary<(string, DateTime), double> Parameters { get; set; }
    }
}
