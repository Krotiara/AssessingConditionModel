using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Interfaces
{

    public enum CommandType
    {
        Assigning,
        VoidCall
    }

    public interface ICommand
    {
        public ParameterNames AssigningParameter { get; }

        public string AssigningParamOriginalName { get; }

        public string OriginCommand { get; }

        CommandType CommandType { get; }
    }
}
