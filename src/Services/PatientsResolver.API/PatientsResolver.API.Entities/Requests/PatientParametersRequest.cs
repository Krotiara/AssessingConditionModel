using Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities.Requests
{
    public class PatientParametersRequest : ILatestParametersrequest
    {
        public string MedicalOrganization { get; set; }
        public int PatientId { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
    }
}
