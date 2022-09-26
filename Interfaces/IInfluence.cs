using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IInfluence
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public DateTime StartTimestamp { get; set; }
   
        public DateTime EndTimestamp { get; set; }

        public InfluenceTypes InfluenceType { get; set; }

        /// <summary>
        /// Название препарата.
        /// </summary>
        public string MedicineName { get; set; }


    }
}
