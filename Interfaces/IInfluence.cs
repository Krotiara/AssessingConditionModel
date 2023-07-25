using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IInfluence
    {
        public string PatientId { get; set; }

        public string Affiliation { get; set; }

        public DateTime StartTimestamp { get; set; }
   
        public DateTime EndTimestamp { get; set; }

        public string InfluenceType { get; set; }

        /// <summary>
        /// Название препарата.
        /// </summary>
        public string MedicineName { get; set; }
    }
}
