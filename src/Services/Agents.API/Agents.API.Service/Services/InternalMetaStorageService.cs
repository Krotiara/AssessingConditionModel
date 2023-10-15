using Agents.API.Entities;
using Agents.API.Entities.Documents;
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
                {SystemCommands.GetAge,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(DateTime), "timestamp") }, typeof(double)) },
                {SystemCommands.Predict,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(double), "age"), (typeof(string), "mlModel")}, typeof(double))}, //TODO - пока заменил на float, но должно быть float[] после добавления взятия по индексу
                {SystemCommands.GetInfluences,
                    new CommandArgsTypesMeta(new List<(Type, string)> {}, typeof(List<Influence>))}
            };
        }

        public ICommandArgsTypesMeta? GetMetaByCommandName(SystemCommands commandName)
        {
            return CommandTypesMeta.ContainsKey(commandName) ? CommandTypesMeta[commandName] : null;
        }
    }
}
