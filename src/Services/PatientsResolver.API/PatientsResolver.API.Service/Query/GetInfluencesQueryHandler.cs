using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Query
{

    public class GetInfluencesQuery : IRequest<List<Influence>>
    {
        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }

        public GetInfluencesQuery(DateTime start, DateTime end)
        {
            StartTimestamp = start;
            EndTimestamp = end;
        }
    }

    public class GetInfluencesQueryHandler : IRequestHandler<GetInfluencesQuery, List<Influence>>
    {
        private readonly IInfluenceRepository influenceRepository;

        public GetInfluencesQueryHandler(IInfluenceRepository influenceRepository)
        {
            this.influenceRepository = influenceRepository;
        }

        public async Task<List<Influence>> Handle(GetInfluencesQuery request, CancellationToken cancellationToken)
        {
            return await influenceRepository.GetInfluences(request.StartTimestamp, request.EndTimestamp);
        }
    }
}
