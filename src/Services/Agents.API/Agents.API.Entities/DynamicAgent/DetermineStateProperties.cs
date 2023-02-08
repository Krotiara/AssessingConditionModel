﻿using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class DetermineStateProperties : IDetermineStateProperties
    {
        public DetermineStateProperties()
        {
            Properties = new Dictionary<ParameterNames, IProperty>();
        }

        public Dictionary<ParameterNames, IProperty> Properties { get; }
    }
}
