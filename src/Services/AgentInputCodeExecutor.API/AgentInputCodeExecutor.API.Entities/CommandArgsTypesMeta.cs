using AgentInputCodeExecutor.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class CommandArgsTypesMeta : ICommandArgsTypesMeta
    {
        public CommandArgsTypesMeta(Type[] inputArgsTypes, Type outputArgType)
        {
            InputArgsTypes = inputArgsTypes;
            OutputArgType = outputArgType;
        }

        public Type[] InputArgsTypes { get; set; }
        public Type OutputArgType { get; set; }
    }
}
