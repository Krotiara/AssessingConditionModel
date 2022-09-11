using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IInfluenceResult
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int InfluenceId { get; set; }

        public IInfluence Influence { get; set; }

        public IEnumerable<IPatientParameter> TrackedParameters { get; set; }

        public double InfluenceEffectiveness { get; set; }
    }
}
