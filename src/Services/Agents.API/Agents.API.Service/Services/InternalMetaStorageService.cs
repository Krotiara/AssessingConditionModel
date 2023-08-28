﻿using Agents.API.Entities;
using Agents.API.Entities.Mongo;
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
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(DateTime), "timestamp") }, typeof(int)) },
                {SystemCommands.Predict,
                    new CommandArgsTypesMeta(new List<(Type, string)> {(typeof(int), "age")}, typeof(float[]))}, 
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
