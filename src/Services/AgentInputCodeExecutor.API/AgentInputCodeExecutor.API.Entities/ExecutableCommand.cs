using AgentInputCodeExecutor.API.Interfaces;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class ExecutableCommand : ICommand
    {
        public ExecutableCommand(string originCommand, CommandType commandType, ParameterNames assigningParameter = ParameterNames.None, string assigningParamOriginalName = null)
        {
            OriginCommand = originCommand;
            CommandType = commandType;
            AssigningParameter = assigningParameter;
            AssigningParamOriginalName = assigningParamOriginalName;
        }

        public string OriginCommand { get; }

        public CommandType CommandType { get; }

        public ParameterNames AssigningParameter { get; }

        public string AssigningParamOriginalName { get; }
    }
}
