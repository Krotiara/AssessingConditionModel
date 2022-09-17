using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Services
{
    public class PatientDataUpdateService : IPatientDataUpdateService
    {

        IMediator mediator;

        public PatientDataUpdateService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public void UpdatePatientData(IPatientData patientData)
        {
            throw new NotImplementedException();
        }
    }
}
