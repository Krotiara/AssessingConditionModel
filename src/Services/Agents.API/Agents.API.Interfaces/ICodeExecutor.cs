using ASMLib.DynamicAgent;
using Interfaces;

namespace Agents.API.Interfaces
{
    public enum ExecuteCodeStatus
    {
        Success = 0,
        Error = 1
    }


    public class ExecuteCodeResult
    {
        public ExecuteCodeStatus Status { get; set; }

        public string ErrorMessage { get; set; }
    }


    public interface ICodeExecutor
    {
        public Task<ExecuteCodeResult> ExecuteCode(string codeLines, IAgent agent,
            IAgentPropertiesNamesSettings commonPropertiesNames,
            CancellationToken cancellationToken=default);


        //public Task<object> ExecuteCommand(SystemCommands command, object[] args, CancellationToken cancellationToken=default);
    }
}
