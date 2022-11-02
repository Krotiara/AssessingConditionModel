using Agents.API.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetAgingStateQueryDb: IRequest<AgingState>
    {
        public int PatientId { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
