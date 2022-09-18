using MediatR;
using PatientsResolver.API.Messaging.Send.Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class SendPatientDataFileSourceCommandHandler : IRequestHandler<SendPatientDataFileSourceCommand>
    {
        private readonly IPatientFileDataSender patientFileDataSender;

        public SendPatientDataFileSourceCommandHandler(IPatientFileDataSender patientFileDataSender)
        {
            this.patientFileDataSender = patientFileDataSender;
        }

        public async Task<Unit> Handle(SendPatientDataFileSourceCommand request, CancellationToken cancellationToken)
        {
            patientFileDataSender.SendPatientsFileData(request.Stream);
            return await Unit.Task;
        }
    }
}
