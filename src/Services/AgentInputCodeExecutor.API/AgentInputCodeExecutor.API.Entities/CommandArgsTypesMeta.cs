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
#warning Нужно инициализацию переделать под строгое соответсвие количества типов и наименований аргументов.
        public CommandArgsTypesMeta(Type[] inputArgsTypes, string[] inputArgsNames, Type outputArgType)
        {
            InputArgsTypes = inputArgsTypes;
            OutputArgType = outputArgType;
            InputArgsNames = inputArgsNames;
        }

        public Type[] InputArgsTypes { get; set; }
        public Type OutputArgType { get; set; }
        public string[] InputArgsNames { get; set; }
    }
}
