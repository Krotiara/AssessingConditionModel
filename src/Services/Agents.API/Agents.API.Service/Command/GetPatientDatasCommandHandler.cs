using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Command
{
    public class GetPatientDatasCommandHandler :
        IRequestHandler<GetPatientDatasCommand, IList<IPatientData<IPatientParameter, IPatient, IInfluence>>>
    {

        private IWebRequester webRequester;

        public GetPatientDatasCommandHandler(IWebRequester webRequester)
        {
            this.webRequester = webRequester;
        }

        public async Task<IList<IPatientData<IPatientParameter, IPatient, IInfluence>>> Handle(GetPatientDatasCommand request, CancellationToken cancellationToken)
        {
            return await webRequester
                .GetResponse<IList<IPatientData<IPatientParameter, IPatient, IInfluence>>>(
                $"http://localhost:62168/patientsData/{request.PatientId}", "GET", "");
        }
    }
}
