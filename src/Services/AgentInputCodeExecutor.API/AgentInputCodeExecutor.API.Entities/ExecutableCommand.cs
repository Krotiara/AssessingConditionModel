using AgentInputCodeExecutor.API.Interfaces;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class Command : ICommand
    {
        public Command(string originCommand, CommandType commandType, ParameterNames assigningParameter = ParameterNames.None)
        {
            OriginCommand = originCommand;
            CommandType = commandType;
            AssigningParameter = assigningParameter;
        }

        public string OriginCommand { get; }

        public CommandType CommandType { get; }

        public ParameterNames AssigningParameter { get; }
    }
}
