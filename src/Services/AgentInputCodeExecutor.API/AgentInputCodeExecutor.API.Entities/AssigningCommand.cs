using AgentInputCodeExecutor.API.Interfaces;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class AssigningCommand : IAssigningCommand
    {
        public AssigningCommand(ParameterNames assigningParameter, string originCommand, CommandType commandType)
        {
            AssigningParameter = assigningParameter;
            OriginCommand = originCommand;
            CommandType = commandType;
        }

        public ParameterNames AssigningParameter { get; }

        public string OriginCommand { get; }

        public CommandType CommandType { get; }
    }
}
