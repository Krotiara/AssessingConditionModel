﻿using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class AddNotExistedPatientsCommandHandler : IRequestHandler<AddNotExistedPatientsCommand, IList<Patient>>
    {
        private readonly PatientsRepository patientsRepository;

        public AddNotExistedPatientsCommandHandler(PatientsRepository patientsRepository)
        {
            this.patientsRepository = patientsRepository;
        }

        public async Task<IList<Patient>> Handle(AddNotExistedPatientsCommand request, CancellationToken cancellationToken)
        {
            //TODO add try catch
            IList<Patient> addedPatients = new List<Patient>();
            foreach(Patient patient in request.Patients)
                if(patientsRepository
                    .GetAll()
                    .FirstOrDefault(x => x.MedicalHistoryNumber ==  patient.MedicalHistoryNumber) == null)
                {
                    await patientsRepository.AddAsync(patient);
                    addedPatients.Add(patient);
                }
            return addedPatients;
        }
    }
}
