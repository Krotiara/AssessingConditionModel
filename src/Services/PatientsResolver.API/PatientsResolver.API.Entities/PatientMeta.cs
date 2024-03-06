using ASMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities
{
    public class PatientMeta : IPatientMeta
    {
        public PatientMeta(string patientId)
        {
            PatientId = patientId;
            InputParametersTimestamps = new();
        }

        public string PatientId { get; set; }
        public List<DateTime> InputParametersTimestamps { get; set; }
    }
}
