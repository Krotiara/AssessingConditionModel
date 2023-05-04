using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Requests
{
    public interface ILatestParametersrequest
    {
        public string MedicalOrganization { get; set; }

        public int PatientId { get; set; }

        public DateTime EndTimestamp { get; set; }

        public List<string> Names { get; set; }
    }
}
