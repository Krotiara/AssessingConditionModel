using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces
{
    public interface IDynamicAgent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IStateDiagram StateDiagram { get; set; }

        public void InitStateDiagram();
    }
}
