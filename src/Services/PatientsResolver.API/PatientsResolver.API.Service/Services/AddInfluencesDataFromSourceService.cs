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

                if(addedPatients.Count > 0)
                    await mediator.Send(new SendPatientsCommand() { Patients = addedPatients.ToList()});

                List<Influence> addedData = 
                    await mediator.Send(new AddInfluenceDataCommand() { Data = data });

#warning Пока убрана отсылка обновления данных о пациентах агентам пациентов
                IUpdatePatientsDataInfo updateInfo = new UpdatePatientsInfo();
                foreach(Influence inf in addedData)
                {
                    updateInfo.UpdateInfo.Add((inf.PatientId, inf.StartTimestamp));
                    updateInfo.UpdateInfo.Add((inf.PatientId, inf.EndTimestamp));
                }
                
                await mediator.Send(new SendUpdatePatientsInfoCommand() { UpdatePatientsInfo = updateInfo }); 
            }
            catch(Exception ex)
            {
                //TODO log
                throw new NotImplementedException();
            }
        }
    }
}
