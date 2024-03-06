using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib
{
    public interface IPatientMeta
    {

        /// <summary>
        /// id of the patient.
        /// </summary>
        public string PatientId { get; set; }

        public List<DateTime> InputParametersTimestamps { get; set; }
    }
}
