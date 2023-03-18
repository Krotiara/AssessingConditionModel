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

    public class AddPatientCommand : IRequest<bool>
    {
        public Patient Patient { get; set; }
    }

    public class AddPatientCommandHandler : IRequestHandler<AddPatientCommand, bool>
    {

        private readonly PatientsRepository patientsRepository;

        public AddPatientCommandHandler(PatientsRepository patientsRepository)
        {
            this.patientsRepository = patientsRepository;
        }

        public async Task<bool> Handle(AddPatientCommand request, CancellationToken cancellationToken)
        {
            if (!IsCorrectPatient(request.Patient))
                throw new AddPatientException("Patient fields is not correct");
            bool isPatientExist = patientsRepository.GetAll().FirstOrDefault(x => x.Id == request.Patient.Id 
                                        && x.MedicalOrganization == request.Patient.MedicalOrganization) != null;
            if (isPatientExist)
                throw new AddPatientException($"Patient with medical history number = {request.Patient.Id} is already exist.");
            try
            {
                await patientsRepository.AddAsync(request.Patient);
                return true;
            }
            catch(Exception ex)
            {
                throw new AddPatientException($"Add patient with with medical history number = {request.Patient.Id} error", ex);
            }
        }


        private bool IsCorrectPatient(Patient patient)
        {
            return patient != null
                //&& patient.Birthday != default(DateTime)  пока убрал, а то в входных данных нет.
                && patient.Id > 0 
                && patient.Gender != Interfaces.GenderEnum.None
                && patient.MedicalOrganization != null;
        }
    }
}
