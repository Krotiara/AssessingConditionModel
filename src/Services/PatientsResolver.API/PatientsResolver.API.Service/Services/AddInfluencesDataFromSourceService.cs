using Interfaces;
using MediatR;
using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Services
{
    public class AddInfluencesDataFromSourceService : IAddInfluencesDataFromSourceService
    {
        private readonly IMediator mediator;

        public AddInfluencesDataFromSourceService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async void AddInfluencesData(List<Influence> data)
        {
            try
            {
                IList<Patient> addedPatients = await mediator.Send(new AddNotExistedPatientsCommand()
                { Patients = data.Select(x => x.Patient).ToList() });

                List<Influence> addedData =
                    await mediator.Send(new AddInfluenceDataCommand() { Data = data });

            }
            catch (Exception ex)
            {
                //TODO log
                throw new AddInfluenceRangeException("Ошибка добавления воздействий в сервисе", ex);
            }
        }
    }
}
