using Agents.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class CommandArgsTypesMeta : ICommandArgsTypesMeta
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputArgs">Значения - тип и имя параметра</param>
        /// <param name="outputArgType"></param>
        public CommandArgsTypesMeta(List<(Type, string)> inputArgs, Type outputArgType)
        {
            InputArgsTypes = inputArgs.Select(x => x.Item1).ToArray();
            OutputArgType = outputArgType;
            InputArgsNames = inputArgs.Select(x => x.Item2).ToArray();
        }

        public Type[] InputArgsTypes { get; set; }
        public Type OutputArgType { get; set; }
        public string[] InputArgsNames { get; set; }
    }
}
