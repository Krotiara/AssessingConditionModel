using Agents.API.Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{
    public class GetAgeCommand : IAgentCommand
    {
        public Delegate Command => async (Dictionary<ParameterNames, PatientParameter> parameters) =>
        {
            if (!parameters.ContainsKey(ParameterNames.Age))
                throw new NotImplementedException(); //TODO - обработка такого случая.
            return int.Parse(parameters[ParameterNames.Age].Value);
        };
    }
}
