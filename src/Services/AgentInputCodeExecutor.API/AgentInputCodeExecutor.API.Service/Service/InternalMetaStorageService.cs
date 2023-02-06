using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Service.Service
{
    public class InternalMetaStorageService : IMetaStorageService
    {
        private Dictionary<string, ICommandArgsTypesMeta> commandTypesMeta { get; }

        public InternalMetaStorageService()
        {
            commandTypesMeta = new Dictionary<string, ICommandArgsTypesMeta>()
            {
                {"GetLatestPatientParams", new CommandArgsTypesMeta( new List<(Type, string)>
                {(typeof(DateTime), "startTimestamp"),(typeof(DateTime), "endTimestamp"),(typeof(int), "patientId")},
                typeof(IList<IPatientParameter>))}
            };
        }

        public ICommandArgsTypesMeta? GetMetaByCommandName(string commandName)
        {
            return commandTypesMeta.ContainsKey(commandName) ? commandTypesMeta[commandName] : null;
        }
    }
}
