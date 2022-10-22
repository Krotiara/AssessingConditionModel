using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IInfluence<T1,T2> where T1: IPatient where T2: IPatientParameter
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public T1 Patient { get; set; }

        public DateTime StartTimestamp { get; set; }
   
        public DateTime EndTimestamp { get; set; }

        public InfluenceTypes InfluenceType { get; set; }

        /// <summary>
        /// Название препарата.
        /// </summary>
        public string MedicineName { get; set; }

        public ConcurrentDictionary<ParameterNames, T2> StartParameters { get; set; }

        public ConcurrentDictionary<ParameterNames, T2> DynamicParameters { get; set; }




    }
}
