using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPatientData
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }


        public IPatient Patient { get; set; }

        public int PatientId { get; set; }

        public int InfluenceId { get; set; }

        public IList<IPatientParameter> Parameters { get; set; }

    }
}
