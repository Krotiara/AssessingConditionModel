using ASMLib.DynamicAgent;

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

        public IAgent Agent { get; }
    }
}
