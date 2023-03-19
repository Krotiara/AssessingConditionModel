using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces
{
    public interface IDataProviderService
    {
        public Task<T> ExecuteSystemCommand<T>(SystemCommands command, object[] args);
    }
}
