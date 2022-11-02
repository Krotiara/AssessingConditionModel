using Agents.API.Entities;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetAllPatientsAgingDynamicsQuery: IRequest<List<IAgingDynamics<AgingPatientState>>>
    {
        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }
    }
}
