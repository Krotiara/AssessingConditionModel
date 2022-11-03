using Agents.API.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetAllInfluencesQuery : IRequest<List<Influence>>
    {
        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }
    }
}
