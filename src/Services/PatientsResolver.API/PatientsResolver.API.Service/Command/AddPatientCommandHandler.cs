using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class AddPatientCommandHandler : IRequestHandler<AddPatientCommand, bool>
    {

        private readonly PatientsRepository patientsRepository;

        public AddPatientCommandHandler(PatientsRepository patientsRepository)
        {
            this.patientsRepository = patientsRepository;
        }

        public async Task<bool> Handle(AddPatientCommand request, CancellationToken cancellationToken)
        {
            bool isPatientExist = patientsRepository.GetAll().FirstOrDefault(x => x.MedicalHistoryNumber == request.Patient.MedicalHistoryNumber) != null;
            if (isPatientExist)
                throw new AddPatientException($"Patient with medical history number = {request.Patient.MedicalHistoryNumber} is already exist.");
            try
            {
                await patientsRepository.AddAsync(request.Patient);
                return true;
            }
            catch(Exception ex)
            {
                throw new AddPatientException($"Add patient with with medical history number = {request.Patient.MedicalHistoryNumber} error", ex);
            }
        }
    }
}
