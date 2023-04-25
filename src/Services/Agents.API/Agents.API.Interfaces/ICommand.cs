using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces
{

    public enum CommandType
    {
        Assigning,
        VoidCall
    }

    public interface ICommand
    {
        public string AssigningParameter { get; }

        public string OriginCommand { get; }

        CommandType CommandType { get; }

        public ConcurrentDictionary<string, IProperty> LocalVariables { get; }

        public ConcurrentDictionary<string, IProperty> LocalProperties { get; }
    }
}
