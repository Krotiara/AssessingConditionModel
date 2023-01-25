using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IExecutableAgentCodeSettings<T>
    {
        public List<string> CodeLines { get; set; }

        public Dictionary<ParameterNames, T> Properties { get; set; }
    }
}
