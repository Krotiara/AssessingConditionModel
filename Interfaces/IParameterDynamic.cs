using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IParameterDynamic
    {
        public IPatientParameter OldPatientParameter { get; set; }

        public IPatientParameter NewPatientParameter { get; set; }
    }
}
