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
                await mediator.Send(new AddPatientDataCommand() { Data = data });
            }
            catch(Exception ex)
            {
                //TODO log
            }
        }
    }
}
