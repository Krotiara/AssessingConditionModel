using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Service.Query
{
    public class GetPatientDataQueryHandler : IRequestHandler<GetPatientDataQuery, List<PatientData>>
    {
        private readonly IPatientDataRepository patientDataRepository;

        public GetPatientDataQueryHandler(IPatientDataRepository patientDatarepository)
        {
            this.patientDataRepository = patientDatarepository;
        }

        public async Task<List<PatientData>> Handle(GetPatientDataQuery request, CancellationToken cancellationToken)
        {
            return await patientDataRepository.GetPatientData(request.PatientId, 
                request.StartTimestamp, request.EndTimestamp);
        }
    }
}
