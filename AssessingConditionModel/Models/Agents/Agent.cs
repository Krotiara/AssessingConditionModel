using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.Agents
{
    public abstract class Agent
    {
        public string Name { get; set; }

        public StateDiagram StateDiagram { get; set; }

        public List<Agent> Connections { get; set; } = new List<Agent>();

        public abstract void InitStateDiagram();

        [DisplayName("Текущее состояние")]
        public string CurrentStateName => StateDiagram.CurrentState.Name;

        public abstract void ProcessMessage(Message message, Agent messenger);

      


        public abstract void ProcessPrivateTransitions();

        public void SendMessage(Message message, Agent agent)
        {
            agent.ReceiveMessage(message, this);
        }

        public void ReceiveMessage(Message message, Agent messenger)
        {
            if (CheckDestination(message))
            {
                ProcessMessage(message, messenger);
            }
        }


        public bool CheckDestination(Message message)
        {
            return Name == message.To;
        }


        public void ConnectTo(Agent agent)
        {
            Connections.Add(agent);
        }


        //public void Disconnect(Agent agent)
        //{
        //    Connections.Remove(agent);   
        //}

    }
}
