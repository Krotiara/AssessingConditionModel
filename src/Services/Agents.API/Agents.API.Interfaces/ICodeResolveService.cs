using Interfaces;
using ASMLib.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASMLib.Entities;

namespace Agents.API.Interfaces
{
    public interface ICodeResolveService
    {
        //Метод принимает команду на псевдокоде и возвращает Func или Action в виде object и мета информацию о входных и выходных типах этого Func или Action для последующего каста.
        public Task<(ICommandArgsTypesMeta?, Delegate)> ResolveCommandAction(ICommand command, 
            AgentPropertiesNamesSettings commonPropertiesNames, CancellationToken cancellationToken);

        public List<object> GetCommandArgsValues(ICommand command, ICommandArgsTypesMeta commandArgsTypesMeta);

        public ICommand ParseCodeLineCommand(string codeLine, IAgent agent);
    }
}
