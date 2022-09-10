using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IInfluenceResult
    {
        public string InfluenceName { get; set; }

        public IEnumerable<IPatientParameter> ParametersDynamic { get; set; }
    }
}
