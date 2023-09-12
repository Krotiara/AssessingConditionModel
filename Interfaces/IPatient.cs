using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPatient
    {
        public string PatientId { get; set; }

        public string Affiliation { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public GenderEnum Gender { get; set; }

        public TreatmentStatus TreatmentStatus { get; set; }

        public ConcurrentDictionary<(string, DateTime), double> Parameters { get; set; }


    }
}
