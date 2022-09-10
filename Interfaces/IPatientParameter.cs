using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPatientParameter
    {
        public string Name { get; set; }

        public object Value { get;  set; }

        public object DynamicValue { get; set; }

    }
}
