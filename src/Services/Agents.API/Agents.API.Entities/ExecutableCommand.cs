using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class ExecutableCommand : ICommand
    {
        public ExecutableCommand(string originCommand, CommandType commandType, Dictionary<string, IProperty> localVariables, string assigningParamOriginalName = null, ParameterNames assigningParameter = ParameterNames.None)
        {
            OriginCommand = originCommand;
            CommandType = commandType;
            AssigningParameter = assigningParameter;
            AssigningParamOriginalName = assigningParamOriginalName;
            LocalVariables = localVariables;
        }

        public string OriginCommand { get; }

        public CommandType CommandType { get; }

        public ParameterNames AssigningParameter { get; }

        public string AssigningParamOriginalName { get; }

        public Dictionary<string, IProperty> LocalVariables { get; }
    }
}
