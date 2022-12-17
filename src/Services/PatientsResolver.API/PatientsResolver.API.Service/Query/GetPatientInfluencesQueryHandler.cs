using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Service.Query
{

    public class GetPatientInfluencesQuery : IRequest<List<Influence>>
    {
        public GetPatientInfluencesQuery(int patientId, DateTime startTimestamp, DateTime endTimestamp)
        {
            PatientId = patientId;
            StartTimestamp = startTimestamp;
            EndTimestamp = endTimestamp;
        }

        public GetPatientInfluencesQuery(int patientId) :
            this(patientId, DateTime.MinValue, DateTime.MaxValue)
        {

        }

        public int PatientId { get; set; }

        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }
    }

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
