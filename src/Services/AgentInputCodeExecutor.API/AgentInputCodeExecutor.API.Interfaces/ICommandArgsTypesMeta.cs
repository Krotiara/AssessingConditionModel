using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Interfaces
{
    public interface ICommandArgsTypesMeta
    {
        public Type[] InputArgsTypes { get; set; }

        public Type OutputArgType { get; set; }
    }
}
