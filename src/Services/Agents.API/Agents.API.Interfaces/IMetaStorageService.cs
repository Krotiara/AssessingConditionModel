using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces
{
    public interface IMetaStorageService
    {
        public ICommandArgsTypesMeta? GetMetaByCommandName(SystemCommands commandName);
    }
}
