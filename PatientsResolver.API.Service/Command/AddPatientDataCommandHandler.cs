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
    public class AddPatientDataCommandHandler : IRequestHandler<AddPatientDataCommand, List<PatientData>>
    {

        private readonly IPatientDataRepository patientDatarepository;

        public AddPatientDataCommandHandler()
        {
            this.patientDatarepository = patientDatarepository;
        }


        public async Task<List<PatientData>> Handle(AddPatientDataCommand request, CancellationToken cancellationToken)
        {
            return await patientDatarepository.AddRangeAsync(request.Data);
        }
    }
}
