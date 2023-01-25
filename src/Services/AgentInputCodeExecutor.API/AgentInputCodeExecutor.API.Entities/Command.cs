using AgentInputCodeExecutor.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class Command : ICommand
    {

        public Command(string originCommand, CommandType commandType)
        {
            OriginCommand = originCommand;
            CommandType = commandType;
        }

        public string OriginCommand { get; }

        public CommandType CommandType { get; }
    }
}
