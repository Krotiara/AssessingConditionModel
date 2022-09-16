using Interfaces;
using MediatR;
using PatientsResolver.API.Messaging.Send.Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class UpdatePatientDataCommandHandler : IRequestHandler<UpdatePatientDataCommand, IPatientData>
    {
        private readonly IPatientDataUpdateSender patientDatasUpdateSender;

        public UpdatePatientDataCommandHandler(IPatientDataUpdateSender patientDatasUpdateSender)
        {
            this.patientDatasUpdateSender = patientDatasUpdateSender;
        }

        public Task<IPatientData> Handle(UpdatePatientDataCommand request, CancellationToken cancellationToken)
        {
            patientDatasUpdateSender.SendPatientData(request.PatientData);
            return (Task<IPatientData>)request.PatientData;
        }
    }
}
