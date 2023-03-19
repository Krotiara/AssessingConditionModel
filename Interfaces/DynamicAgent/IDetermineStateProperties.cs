﻿using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DynamicAgent
{
    public interface IDetermineStateProperties
    {
        public Dictionary<string, IProperty> Properties { get; }

        public DateTime Timestamp { get; }
    }
}
