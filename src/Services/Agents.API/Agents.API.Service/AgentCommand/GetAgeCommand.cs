using Agents.API.Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{
    //TODO преобразовать в получение по дате рождения и переданному времени.
    public class GetAgeCommand : IAgentCommand
    {
        public Delegate Command => async (Dictionary<string, PatientParameter> parameters) =>
        {
            if (!parameters.ContainsKey(ParameterNames.Age))
                throw new NotImplementedException(); //TODO - обработка такого случая.
            return int.Parse(parameters[ParameterNames.Age].Value);
        };
    }
}
