using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Query
{
    public class GetPatientQuery : IRequest<Patient>
    {
        public int PatientId { get; set; }

        public string MedicalOrganization { get; set; }
    }

    public class GetPatientQueryHandler: IRequestHandler<GetPatientQuery, Patient>
    {
        private readonly PatientsRepository patientDataRepository;

        public GetPatientQueryHandler(PatientsRepository patientDataRepository)
        {
            this.patientDataRepository = patientDataRepository;
        }

        public async Task<Patient> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {
           Patient? p =  await patientDataRepository.GetPatientBy(request.PatientId, request.MedicalOrganization);
            if (p == null)
                throw new PatientNotFoundException($"Не найден пациент с id = {request.PatientId}");
            else return p;
        }
           
    }
}
