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
    public class GetLatesPatientParametersQueryHandler : IRequestHandler<GetLatesPatientParametersQuery, List<PatientParameter>>
    {
        private readonly PatientParametersRepository repository;
        public GetLatesPatientParametersQueryHandler(PatientParametersRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<PatientParameter>> Handle(GetLatesPatientParametersQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetLatestParameters(request.PatientId);
        }
    }
}
