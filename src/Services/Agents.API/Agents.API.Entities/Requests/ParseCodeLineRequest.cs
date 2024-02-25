using ASMLib.Entities;
using System.Collections.Concurrent;

namespace Agents.API.Entities.Requests
{
    public class ParseCodeLineRequest
    {
        public string CodeLine { get; set; }

        public ConcurrentDictionary<string, Property> LocalVariables { get; }

        public ConcurrentDictionary<string, Property> LocalProperties { get; }

        public ParseCodeLineRequest(string codeLine,
            ConcurrentDictionary<string, Property> localVariables,
            ConcurrentDictionary<string, Property> localProperties)
        {
            CodeLine = codeLine;
            LocalVariables = localVariables;
            LocalProperties = localProperties;
        }
    }
}
