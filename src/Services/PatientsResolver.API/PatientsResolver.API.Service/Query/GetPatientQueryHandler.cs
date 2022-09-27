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
    public class GetPatientQueryHandler: IRequestHandler<GetPatientQuery, Patient>
    {
        private readonly IPatientDataRepository patientDataRepository;

        public GetPatientQueryHandler(IPatientDataRepository patientDataRepository)
        {
            this.patientDataRepository = patientDataRepository;
        }

        public async Task<Patient> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {
            return await patientDataRepository.GetPatientDataByIdAsync(request.PatientId, cancellationToken);
        }
    }
}