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
    public class AddNotExistedPatientsCommand : IRequest<IList<Patient>>
    {
        public IList<Patient> Patients { get; set; }
    }


    public class AddNotExistedPatientsCommandHandler : IRequestHandler<AddNotExistedPatientsCommand, IList<Patient>>
    {
        private readonly PatientsRepository patientsRepository;
        private readonly IMediator mediator;

        public AddNotExistedPatientsCommandHandler(PatientsRepository patientsRepository, IMediator mediator)
        {
            this.patientsRepository = patientsRepository;
            this.mediator = mediator;
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
                            patient.Gender == Interfaces.GenderEnum.None
                            /*patient.Birthday == default(DateTime)*/) //Пока убрал, так как в входных данных нет. 14.12.2022
                                throw new AddPatientException($"Some requied values is empty for patient with history number = {patient.MedicalHistoryNumber}");

                        bool isAdded = await mediator.Send(new AddPatientCommand() { Patient = patient }); /* patientsRepository.AddAsync(patient);*/
                        if (isAdded)
                            addedPatients.Add(patient);
                        else throw new AddPatientException($"Patient with history number = {patient.MedicalHistoryNumber} was not added");
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
