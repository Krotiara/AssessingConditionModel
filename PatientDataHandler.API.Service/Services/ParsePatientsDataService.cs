using Interfaces;
using MediatR;
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

        public async void ParsePatients(Stream fileStream)
        {
            try
            {
                await mediator.Send(new SendPatientsDataFileCommand() { FileStream = fileStream });
            }
            catch (Exception ex)
            {
                //TODO log
            }
        }
    }
}
