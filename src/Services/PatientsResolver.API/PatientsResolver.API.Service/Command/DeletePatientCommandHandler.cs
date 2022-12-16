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

    public class DeletePatientCommand : IRequest<bool>
    {
        public int PatientId { get; set; }

        public DeletePatientCommand(int patientId)
        {
            PatientId = patientId;
        }
    }

    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, bool>
    {

        private readonly PatientsRepository patientsRepository;

        public DeletePatientCommandHandler(PatientsRepository patientsRepository)
        {
            this.patientsRepository = patientsRepository;
        }


        public async Task<bool> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            Patient? patient = patientsRepository.GetAll().FirstOrDefault(x => x.MedicalHistoryNumber == request.PatientId);
            if (patient == null)
                throw new DeletePatientException($"Пациент с id = {request.PatientId} не найден");
            try
            {
                await patientsRepository.DeleteAsync(patient);
            }
            catch(Exception ex)
            {
                throw new DeletePatientException($"Удаление пациента с id = {request.PatientId}", ex);
            }
            return true;
        }
    }
}
