using MediatR;
using PatientsResolver.API.Messaging.Send.Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class SendPatientDataFileSourceCommandHandler : IRequestHandler<SendPatientDataFileSourceCommand, bool>
    {
        private readonly IPatientFileDataSender patientFileDataSender;

        public SendPatientDataFileSourceCommandHandler(IPatientFileDataSender patientFileDataSender)
        {
            this.patientFileDataSender = patientFileDataSender;
        }

        public async Task<bool> Handle(SendPatientDataFileSourceCommand request, CancellationToken cancellationToken)
        {
            bool isSuccess = patientFileDataSender.SendPatientsFileData(request.Data);
            return isSuccess;
        }
    }
}
