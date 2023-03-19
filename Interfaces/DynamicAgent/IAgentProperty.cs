using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DynamicAgent
{
    public interface IProperty
    {
        public string OriginName { get; set; }

        public ParameterNames Name { get; set; }

        public Type Type { get; set; }

        public object Value { get; set; }
    }
}
