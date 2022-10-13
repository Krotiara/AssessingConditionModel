using Agents.API.Entities;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Command
{
    public class InitPatientAgentsCommand: IRequest<IList<AgentPatient>>
    {
        public HashSet<IPatient> Patients { get; set; }
    }
}
