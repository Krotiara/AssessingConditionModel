using Agents.API.Entities;
using Agents.API.Interfaces;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class InternalMetaStorageService : IMetaStorageService
    {
        private Dictionary<SystemCommands, ICommandArgsTypesMeta> CommandTypesMeta { get; }

        //todo в бд
        public InternalMetaStorageService()
        {
            CommandTypesMeta = new Dictionary<SystemCommands, ICommandArgsTypesMeta>()
            {
                {SystemCommands.GetLatestPatientParameters,
                    new CommandArgsTypesMeta( new List<(Type, string)> {}, typeof(Dictionary<string,PatientParameter>))},
                {SystemCommands.GetAge,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(DateTime), "timestamp") }, typeof(int)) },
                {SystemCommands.GetBioageByFunctionalParameters,
                    new CommandArgsTypesMeta(new List<(Type, string)> {}, typeof(int))}, 
                {SystemCommands.GetInfluences,
                    new CommandArgsTypesMeta(new List<(Type, string)> {}, typeof(List<Influence>))},
                {SystemCommands.GetInfluencesWithoutParameters,
                    new CommandArgsTypesMeta(new List<(Type, string)> {}, typeof(List<Influence>))},
                {SystemCommands.GetDentistSum,new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(int), "age")}, typeof(int)) }
            };
        }

        public ICommandArgsTypesMeta? GetMetaByCommandName(SystemCommands commandName)
        {
            return CommandTypesMeta.ContainsKey(commandName) ? CommandTypesMeta[commandName] : null;
        }
    }
}
