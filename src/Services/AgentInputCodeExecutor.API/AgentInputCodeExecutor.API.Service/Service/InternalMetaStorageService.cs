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
        private Dictionary<SystemCommands, ICommandArgsTypesMeta> commandTypesMeta { get; }

        public InternalMetaStorageService()
        {
            commandTypesMeta = new Dictionary<SystemCommands, ICommandArgsTypesMeta>()
            {
                {SystemCommands.GetLatestPatientParameters,
                    new CommandArgsTypesMeta( new List<(Type, string)> {(typeof(DateTime), "startTimestamp"),
                        (typeof(DateTime), "endTimestamp"),
                        (typeof(int), "patientId")}, typeof(IList<IPatientParameter>))},
                {SystemCommands.GetAge, new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(List<PatientParameter>),"parameters") }, typeof(double)) },
                {SystemCommands.GetBioage, new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(List<PatientParameter>),"parameters") }, typeof(double)) },
                {SystemCommands.GetAgeRangBy, new CommandArgsTypesMeta(new List<(Type, string)>{(typeof(double),"age"),(typeof(double),"bioAge")}, typeof(double)) }
            };
        }

        public ICommandArgsTypesMeta? GetMetaByCommandName(SystemCommands commandName)
        {
            return commandTypesMeta.ContainsKey(commandName) ? commandTypesMeta[commandName] : null;
        }
    }
}
