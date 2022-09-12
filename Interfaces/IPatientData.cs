using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPatientData
    {
        public int PatientId { get; set; }

        public int InfluenceId { get; set; }

        public IInfluence Influence { get; set; }

        public IEnumerable<IPatientParameter> Parameters { get; set; }

    }
}
