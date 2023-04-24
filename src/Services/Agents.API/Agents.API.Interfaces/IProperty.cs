using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces
{
    public interface IProperty
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public object Value { get; set; }
    }
}
