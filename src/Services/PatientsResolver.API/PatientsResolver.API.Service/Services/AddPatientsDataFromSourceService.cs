using Agents.API.Entities;
using Interfaces;
using MediatR;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Services
{
    public class AddPatientsDataFromSourceService : IAddPatientsDataFromSourceService
    {
        private readonly IMediator mediator;

        public AddPatientsDataFromSourceService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async void AddPatientsData(List<PatientData> data)
        {
            try
            {
#warning error на данный момент Patient из входных данных = null.
                IList<Patient> addedPatients = 
                    await mediator.Send(new AddNotExistedPatientsCommand() 
                    { Patients = data.Select(x => x.Patient).ToList() });

                if(addedPatients.Count > 0)
                    await mediator.Send(new SendPatientsCommand() { Patients = addedPatients.ToList()});

                List<PatientData> addedData = 
                    await mediator.Send(new AddPatientDataCommand() { Data = data });

                IUpdatePatientsInfo updateInfo = new UpdatePatientsInfo() 
                { UpdatedIds = new HashSet<int>(addedData.Select(x => x.PatientId)) };
                await mediator.Send(new SendUpdatePatientsInfoCommand() { UpdatePatientsInfo = updateInfo }); 
            }
            catch(Exception ex)
            {
                //TODO log
            }
        }
    }
}
