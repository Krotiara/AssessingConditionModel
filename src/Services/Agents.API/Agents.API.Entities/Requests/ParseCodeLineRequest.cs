using ASMLib.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests
{
    public class ParseCodeLineRequest
    {
        public string CodeLine { get; set; }

        public ConcurrentDictionary<string, IProperty> LocalVariables { get; }

        public ConcurrentDictionary<string, IProperty> LocalProperties { get; }

        public ParseCodeLineRequest(string codeLine,
            ConcurrentDictionary<string, IProperty> localVariables,
            ConcurrentDictionary<string, IProperty> localProperties)
        {
            CodeLine = codeLine;
            LocalVariables = localVariables;
            LocalProperties = localProperties;
        }
    }
}
