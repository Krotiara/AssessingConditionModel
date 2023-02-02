using AgentInputCodeExecutor.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Interfaces
{
    public interface ICodeResolveService
    {
        //Метод принимает команду на псевдокоде и возвращает Func или Action в виде object и мета информацию о входных и выходных типах этого Func или Action для последующего каста.
        public Task<(ICommandArgsTypesMeta, Delegate)> ResolveCommandAction(ICommand command, CancellationToken cancellationToken);
    }
}
