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
        private Dictionary<SystemCommands, ICommandArgsTypesMeta> commandTypesMeta { get; }

        public InternalMetaStorageService()
        {
            commandTypesMeta = new Dictionary<SystemCommands, ICommandArgsTypesMeta>()
            {
                {SystemCommands.GetLatestPatientParameters,
                    new CommandArgsTypesMeta( new List<(Type, string)> {(typeof(DateTime), "startTimestamp"),
                        (typeof(DateTime), "endTimestamp"),
                        (typeof(int), "patientId")}, typeof(IList<IPatientParameter>))},
                {SystemCommands.GetAge,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(Dictionary<ParameterNames,PatientParameter>),"parameters") }, typeof(int)) },
                {SystemCommands.GetBioageByFunctionalParameters,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(Dictionary<ParameterNames, PatientParameter>),"parameters") }, typeof(int)) },
                {SystemCommands.GetAgeRangBy,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(int),"age"),(typeof(int),"bioAge")}, typeof(int)) },
                {SystemCommands.GetInfluences,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(DateTime),"start"),
                        (typeof(DateTime),"end"),
                        (typeof(int),"observedId") }, typeof(List<Influence>))},
                {SystemCommands.GetInfluencesWithoutParameters,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(DateTime),"start"),
                        (typeof(DateTime),"end"),
                        (typeof(int),"observedId") }, typeof(List<Influence>))}                  
            };
        }

        public ICommandArgsTypesMeta? GetMetaByCommandName(SystemCommands commandName)
        {
            return commandTypesMeta.ContainsKey(commandName) ? commandTypesMeta[commandName] : null;
        }
    }
}
