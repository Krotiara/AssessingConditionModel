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
            if (!request.Patients.Any(x => x != null))
                throw new NotImplementedException(); //TODO throw ex

            
            List<string> exMessages = new List<string>();
            IList<Patient> addedPatients = new List<Patient>();
            foreach(Patient patient in request.Patients)
                if(patientsRepository
                    .GetAll()
                    .FirstOrDefault(x => x.MedicalHistoryNumber ==  patient.MedicalHistoryNumber) == null)
                {
                    try
                    {
#warning По идее лучше сделать правилами при заполнении, а то выглядит костыльно.
                        if (patient.MedicalHistoryNumber <= 0 || 
                            patient.Gender == Interfaces.GenderEnum.None || 
                            patient.Birthday == default(DateTime))
                                throw new AddPatientException($"Some requied values is empty for patient with history number = {patient.MedicalHistoryNumber}");

                        await patientsRepository.AddAsync(patient);
                        addedPatients.Add(patient);
                    }
                    catch(Exception ex)
                    {
                        exMessages.Add($"Patient with history = {patient.MedicalHistoryNumber}:{ex}.");
                        continue;
                    }
                }
            if (exMessages.Count > 0)
                throw new AddPatientsRangeException(string.Join("\n", exMessages));
            else
                return addedPatients;
        }
    }
}
