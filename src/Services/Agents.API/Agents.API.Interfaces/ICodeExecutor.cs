using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces
{ 
    public interface ICodeExecutor
    {
        public Task<ConcurrentDictionary<string, IProperty>> ExecuteCode(string codeLines, 
            ConcurrentDictionary<string, IProperty> variables, CancellationToken cancellationToken=default);


        //public Task<object> ExecuteCommand(SystemCommands command, object[] args, CancellationToken cancellationToken=default);
    }
}
