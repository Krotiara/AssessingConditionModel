﻿using Agents.API.Entities.Documents;
using ASMLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class CommandResult
    {
        public CommandResult(object result, IEnumerable<Parameter> buffer = null)
        {
            Result = result;
            CommandBuffer = buffer;
        }

        public CommandResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public object Result { get; set; }

        public IEnumerable<Parameter> CommandBuffer { get; }

        public string ErrorMessage { get; set; }

        public bool IsError => ErrorMessage != null && ErrorMessage != string.Empty;
    }
}
