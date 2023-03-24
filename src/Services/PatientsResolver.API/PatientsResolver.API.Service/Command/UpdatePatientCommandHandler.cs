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

    public class UpdatePatientCommand : IRequest<Patient>
    {
        public UpdatePatientCommand(Patient patient)
        {
            Patient = patient;
        }

        public Patient Patient { get; }
    }

    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Patient>
    {
        private readonly PatientsRepository patientsRepository;

        public UpdatePatientCommandHandler(PatientsRepository patientsRepository)
        {
            this.patientsRepository = patientsRepository;
        }

        public async Task<Patient> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            if (!IsCorrectPatient(request.Patient))
                throw new UpdatePatientException($"Не валидные данные пациента: " +
                    $"birthday - {request.Patient.Birthday}, " +
                    $"medical history number - {request.Patient.Id}, " +
                    $"gender - {request.Patient.Gender}.");
            Patient? patient = patientsRepository.GetAll().FirstOrDefault(x => x.Id == request.Patient.Id 
                                                                            && x.MedicalOrganization == request.Patient.MedicalOrganization);
            if (patient == null)
                throw new UpdatePatientException($"Пациент с id = {request.Patient.Id} не найден для изменения");
            SetNewValues(request.Patient, patient);           
            try
            {
                await patientsRepository.UpdateAsync(patient);
            }
            catch (Exception ex)
            {
                throw new UpdatePatientException("Ошибка сохранения изменений пациента", ex);
            }
            return patient;
        }


        private bool IsCorrectPatient(Patient patient)
        {
            return patient != null
                //&& patient.Birthday != default(DateTime) пока убрал, а то в входных данных нет.
                && patient.Id > 0;
        }


        private void SetNewValues(Patient from, Patient to)
        {
            //TODO Заменить на reflection
            to.Name = from.Name;
            to.Birthday = from.Birthday;
            to.Gender = from.Gender;
        }
    }
}
