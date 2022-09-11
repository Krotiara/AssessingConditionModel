using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IInfluenceResult
    {
        public int InfluenceId { get; set; }

        public IEnumerable<IPatientParameter> TrackedParameters { get; set; }

        public double InfluenceEffectiveness { get; set; }
    }
}
