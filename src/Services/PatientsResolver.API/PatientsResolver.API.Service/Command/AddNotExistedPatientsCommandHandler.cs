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

            request.Patients.Add(new Patient() { MedicalHistoryNumber = 434344144, Name = "test", Birthday = DateTime.MaxValue });

            IList<Patient> addedPatients = new List<Patient>();
            foreach(Patient patient in request.Patients)
                if(patientsRepository
                    .GetAll()
                    .FirstOrDefault(x => x.MedicalHistoryNumber ==  patient.MedicalHistoryNumber) == null)
                {
                    try
                    {
                        await patientsRepository.AddAsync(patient);
                        addedPatients.Add(patient);
                    }
                    catch(Exception ex)
                    {
                        //TODO try catch with custom exceptions
                        continue;
                    }
                }
            return addedPatients;
        }
    }
}
