using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DynamicAgent
{
    public interface IDynamicAgent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IDynamicAgentInitSettings Settings { get; }

        public void UpdateState();
    }
}
