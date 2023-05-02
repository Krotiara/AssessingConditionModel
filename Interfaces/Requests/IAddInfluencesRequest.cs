using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Requests
{
    public interface IAddInfluencesRequest
    {
        public string MedicineName { get; set; }

        public DateTime StartTimestamp { get; set; }

        public DateTime? EndTimestamp { get; set; }

        public string Affiliation { get; set; }

        public byte[] Content { get; set; }
    }
}
