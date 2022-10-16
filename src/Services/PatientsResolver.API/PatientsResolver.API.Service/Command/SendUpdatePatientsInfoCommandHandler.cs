using MediatR;
using PatientsResolver.API.Messaging.Send.Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class SendUpdatePatientsInfoCommandHandler :
        IRequestHandler<SendUpdatePatientsInfoCommand, Unit>
    {

        IUpdatePatientsSender updatePatientsSender;

        public SendUpdatePatientsInfoCommandHandler(IUpdatePatientsSender updatePatientsSender)
        {
            this.updatePatientsSender = updatePatientsSender;
        }

        public async Task<Unit> Handle(SendUpdatePatientsInfoCommand request, CancellationToken cancellationToken)
        {
            updatePatientsSender.SendUpdatePatientsInfo(request.UpdatePatientsInfo);
            return await Unit.Task;
        }
    }
}
