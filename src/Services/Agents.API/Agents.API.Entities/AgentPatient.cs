using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class AgentPatient : IAgent
    {
        private readonly IWebRequester webRequester;
        public AgentPatient(IWebRequester webRequester)
        {
            this.webRequester = webRequester;
        }

        public AgentPatient() { }

        public int Id { get; set; }
        public string Name { get ; set ; }
        public StateDiagram StateDiagram { get ; set ; }
        public List<IAgent> Connections { get ; set ; }
        

        public void InitStateDiagram()
        {
            throw new NotImplementedException();
        }

        public void ProcessPrivateTransitions()
        {
            throw new NotImplementedException();
        }
    }
}
