using MediatR;
using PatientsResolver.API.Messaging.Send.Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class SendPatientsCommandHandler : IRequestHandler<SendPatientsCommand, Unit>
    {
        private readonly IPatientsSender patientsSender;

        public SendPatientsCommandHandler(IPatientsSender patientsSender)
        {
            this.patientsSender = patientsSender;
        }

        public async Task<Unit> Handle(SendPatientsCommand request, CancellationToken cancellationToken)
        {
            patientsSender.SendPatients(request.Patients);
            return await Unit.Task;
        }
    }
}
