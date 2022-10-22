using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Service.Query
{
    public class GetPatientInfluencesQueryHandler : IRequestHandler<GetPatientInfluencesQuery, List<Influence>>
    {
        private readonly IInfluenceRepository influenceRepository;

        public GetPatientInfluencesQueryHandler(IInfluenceRepository influenceRepository)
        {
            this.influenceRepository = influenceRepository;
        }

        public async Task<List<Influence>> Handle(GetPatientInfluencesQuery request, CancellationToken cancellationToken)
        {
            return await influenceRepository.GetPatientInfluences(request.PatientId, 
                request.StartTimestamp, request.EndTimestamp);
        }
    }
}
