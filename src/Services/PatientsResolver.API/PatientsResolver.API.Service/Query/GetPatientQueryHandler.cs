﻿using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
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
    }

    public class GetPatientQueryHandler: IRequestHandler<GetPatientQuery, Patient>
    {
        private readonly PatientsRepository patientDataRepository;

        public GetPatientQueryHandler(PatientsRepository patientDataRepository)
        {
            this.patientDataRepository = patientDataRepository;
        }

        public async Task<Patient> Handle(GetPatientQuery request, CancellationToken cancellationToken) => 
            await patientDataRepository.GetPatientBy(request.PatientId);
    }
}
