using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Service.Query
{

    public class GetPatientInfluencesQuery : IRequest<List<Influence>>
    {
        public GetPatientInfluencesQuery(int patientId, string medicalOrganization, DateTime startTimestamp, DateTime endTimestamp, bool includeParams = true)
        {
            PatientId = patientId;
            StartTimestamp = startTimestamp;
            EndTimestamp = endTimestamp;
            IncludeParams = includeParams;
            MedicalOrganization = medicalOrganization;

        }

        public GetPatientInfluencesQuery(int patientId, string medicalOrganization, bool includeParams = true) :
            this(patientId, medicalOrganization, DateTime.MinValue, DateTime.MaxValue, includeParams)
        {

        }

        public int PatientId { get; set; }

        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }

        public bool IncludeParams { get; set; }

        public string MedicalOrganization { get; set; }
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
            return await influenceRepository.GetPatientInfluences(request.PatientId, request.MedicalOrganization,
                request.StartTimestamp, request.EndTimestamp, request.IncludeParams);
        }
    }
}
