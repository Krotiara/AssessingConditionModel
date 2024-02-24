using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.DynamicAgent
{
    public interface IProperty
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public object Value { get; set; }
    }
}
