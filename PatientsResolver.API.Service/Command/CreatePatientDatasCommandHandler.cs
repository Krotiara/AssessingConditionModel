using Interfaces;
using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class CreatePatientDatasCommandHandler : IRequestHandler<CreatePatientDatasCommand, List<PatientData>>
    {

        private readonly IPatientDatarepository patientDataRepository;

        public CreatePatientDatasCommandHandler(IPatientDatarepository patientDataRepository)
        {
            this.patientDataRepository = patientDataRepository;
        }

        public async Task<List<PatientData>> Handle(CreatePatientDatasCommand request, CancellationToken cancellationToken)
        {
            return await patientDataRepository.AddRangeAsync(request.PatientDatas);
        }
    }
}
