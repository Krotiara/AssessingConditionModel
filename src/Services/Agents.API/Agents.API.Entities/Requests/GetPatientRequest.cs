using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests
{
    public class GetPatientRequest
    {
        public string Affiliation { get; set; }

        public string PatientId { get; set; }
    }
}
