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
                List<PatientData> addedData = 
                    await mediator.Send(new AddPatientDataCommand() { Data = data });

                IUpdatePatientsInfo updateInfo = new UpdatePatientsInfo() 
                { UpdatedIds = new HashSet<int>(addedData.Select(x => x.PatientId)) };
                await mediator.Send(new SendUpdatePatientsInfoCommand() { UpdatePatientsInfo = updateInfo });

                //TODO отлов добавленных пациентов. Пока дял теста берем все
                List<Patient> testPatients = addedData.Select(x => x.Patient).ToList();
                await mediator.Send(new SendPatientsCommand() { Patients = testPatients });
            }
            catch(Exception ex)
            {
                //TODO log
            }
        }
    }
}
