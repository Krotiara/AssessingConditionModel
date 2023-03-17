using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    internal interface ICodeExecutor
    {
        public Task<Dictionary<string, IProperty>> ExecuteCode(string codeLines);


        public Task<object> ExecuteCommand(string commandName, object[] args);
    }
}
