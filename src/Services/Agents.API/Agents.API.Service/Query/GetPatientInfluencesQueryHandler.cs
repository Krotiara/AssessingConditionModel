using Agents.API.Entities;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetPatientInfluencesQueryHandler: IRequestHandler<GetPatientInfluencesQuery, List<Influence>>
    {
        private readonly IWebRequester webRequester;

        public GetPatientInfluencesQueryHandler(IWebRequester webRequester)
        {
            this.webRequester = webRequester;
        }

        public async Task<List<Influence>> Handle(GetPatientInfluencesQuery request, CancellationToken cancellationToken)
        {
            string url = $"{Environment.GetEnvironmentVariable("PATIENTRESOLVER_API_URL")}/influence/{request.PatientId}";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { request.StartTimestamp, request.EndTimestamp });
            return await webRequester.GetResponse<List<Influence>>(url, "POST", body);
        }
    }
}
