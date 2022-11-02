using MediatR;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Query
{
    public class GetInfluencesQuery: IRequest<List<Influence>>
    {
        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }

        public GetInfluencesQuery(DateTime start, DateTime end)
        {
            StartTimestamp = start;
            EndTimestamp = end;
        }
    }
}
