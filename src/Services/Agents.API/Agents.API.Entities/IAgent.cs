using Interfaces;
using System.ComponentModel;
using Agents.API.Entities;

namespace Agents.API.Entities
{
    public interface IAgent
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public StateDiagram StateDiagram { get; set; }

        public void InitStateDiagram();
    }
}