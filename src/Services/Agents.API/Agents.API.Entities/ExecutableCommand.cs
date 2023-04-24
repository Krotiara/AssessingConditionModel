using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class ExecutableCommand : ICommand
    {
        public ExecutableCommand(string originCommand, CommandType commandType, 
            ConcurrentDictionary<string, IProperty> localVariables, string assigningParamOriginalName = null)
        {
            OriginCommand = originCommand;
            CommandType = commandType;
            AssigningParameter = assigningParamOriginalName;
            LocalVariables = localVariables;
        }

        public string OriginCommand { get; }

        public CommandType CommandType { get; }

        public string AssigningParameter { get; }

        public ConcurrentDictionary<string, IProperty> LocalVariables { get; }
    }
}
