using Agents.API.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetAgingStateQuery: IRequest<AgingPatientState>
    {
        public int PatientId { get; set; }
    }
}
