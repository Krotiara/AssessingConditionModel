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

        //todo в бд
        public InternalMetaStorageService()
        {
            commandTypesMeta = new Dictionary<SystemCommands, ICommandArgsTypesMeta>()
            {
                {SystemCommands.GetLatestPatientParameters,
                    new CommandArgsTypesMeta( new List<(Type, string)> {(typeof(DateTime), "startTimestamp"),
                        (typeof(DateTime), "endTimestamp"),
                        (typeof(int), "patientId"),
                        (typeof(string), "medicalOrganization")}, 
                        typeof(Dictionary<string,PatientParameter>))},
                {SystemCommands.GetAge,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(Dictionary<string,PatientParameter>),"parameters") }, typeof(int)) },
                {SystemCommands.GetBioageByFunctionalParameters,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(string),"patientId"),
                        (typeof(string), "patientAffiliation"),
                        (typeof(DateTime), "endTimestamp")}, typeof(int))},
                {SystemCommands.GetAgeRangBy,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(int),"age"),(typeof(int),"bioAge")}, typeof(int)) },
                {SystemCommands.GetInfluences,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(DateTime),"start"),
                        (typeof(DateTime),"end"),
                        (typeof(int),"observedId"),
                        (typeof(string), "medicalOrganization")}, typeof(List<Influence>))},
                {SystemCommands.GetInfluencesWithoutParameters,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(DateTime),"start"),
                        (typeof(DateTime),"end"),
                        (typeof(int),"observedId"),
                        (typeof(string), "medicalOrganization")}, typeof(List<Influence>))},
                {SystemCommands.GetDentistSum,new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(Dictionary<string, PatientParameter>),"parameters") }, typeof(int)) }
            };
        }

        public ICommandArgsTypesMeta? GetMetaByCommandName(SystemCommands commandName)
        {
            return commandTypesMeta.ContainsKey(commandName) ? commandTypesMeta[commandName] : null;
        }
    }
}
