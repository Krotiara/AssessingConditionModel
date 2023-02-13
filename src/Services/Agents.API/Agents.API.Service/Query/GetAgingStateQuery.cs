using Agents.API.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetAgingStateQuery: IRequest<AgingState>
    {

        public GetAgingStateQuery(int patientId, DateTime timestamp)
        {
            PatientId = patientId;
            Timestamp = timestamp;
        }

        public int PatientId { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
