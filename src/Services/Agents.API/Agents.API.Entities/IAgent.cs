﻿using Interfaces;
using System.ComponentModel;
using Agents.API.Entities;

namespace Agents.API.Entities
{
    public interface IAgent
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public StateDiagram StateDiagram { get; set; }

        public List<IAgent> Connections { get; set; }

        public void InitStateDiagram();

      //  public void ProcessMessage(Message message);


        public void ProcessPrivateTransitions();

       // public void SendMessage(Message message);
       

        //public bool CheckDestination(Message message)
        //{
        //    return Name == message.To;
        //}


        public void ConnectTo(IAgent agent)
        {
            Connections.Add(agent);
        }


        //public void Disconnect(Agent agent)
        //{
        //    Connections.Remove(agent);   
        //}

    }
}