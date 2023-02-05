using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IExecutableAgentCodeSettings
    {
        public List<string> CodeLines { get; set; }

        public Dictionary<string, IProperty> Properties { get; set; }
    }
}
