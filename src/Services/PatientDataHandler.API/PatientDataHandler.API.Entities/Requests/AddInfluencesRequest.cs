using Interfaces;
using Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API.Entities.Requests
{
    public class AddInfluencesRequest : IAddInfluencesRequest
    {
        public string MedicineName { get; set; }

        public DateTime StartTimestamp { get; set; }

        public DateTime? EndTimestamp { get; set; }

        public string Affiliation { get; set; }

        public byte[] Content { get; set; }
        public InputFileType InputFileType { get; set; }
        public string InfluenceType { get; set; }
    }
}
