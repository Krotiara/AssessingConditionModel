using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces
{
    public interface ICommandArgsTypesMeta
    {
        public Type[] InputArgsTypes { get; set; }

        public string[] InputArgsNames { get; set; }

        public Type OutputArgType { get; set; }
    }
}
