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
    public class GetPatientInfluencesQueryHandler :
        IRequestHandler<GetPatientInfluencesQuery, List<Influence>>
    {

        private InfluenceRepository influenceRepository;


        public GetPatientInfluencesQueryHandler(InfluenceRepository influenceRepository)
        {
            this.influenceRepository = influenceRepository;
        }

        public async Task<List<Influence>> Handle(GetPatientInfluencesQuery request, CancellationToken cancellationToken)
        {
            return influenceRepository
                .GetAll()
                .Where(x => x.PatientId == request.PatientId &&
                       x.StartTimestamp >= request.StartTimestamp &&
                       x.EndTimestamp <= request.EndTimestamp)
                .ToList();
        }
    }
}
