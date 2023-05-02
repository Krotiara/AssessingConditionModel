using Interfaces;
using Interfaces.Requests;
using MediatR;
using PatientDataHandler.API.Entities;
using PatientDataHandler.API.Service.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API.Service.Services
{
    public class ParsePatientsDataService : IParsePatientsDataService
    {

        private readonly IMediator mediator;

        public ParsePatientsDataService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async void ParsePatients(IAddInfluencesRequest request)
        {
            try
            {
                await mediator.Send(new SendPatientsDataFileCommand() { Request = request });
            }
            catch (Exception ex)
            {
                //TODO log
            }
        }
    }
}
