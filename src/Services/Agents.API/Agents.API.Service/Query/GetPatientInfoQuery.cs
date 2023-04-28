using Agents.API.Entities;
using Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetPatientInfoQuery: IRequest<HttpResponseMessage>
    {
        public string Id { get; set; }

        public string Organization { get; set; }
    }


    public class GetPatientInfoQueryHandler : IRequestHandler<GetPatientInfoQuery, HttpResponseMessage>
    {
        private readonly IWebRequester _webRequester;
        private readonly string _patientsResolverApiUrl;

        public GetPatientInfoQueryHandler(IWebRequester webRequester, IOptions<EnvSettings> settings)
        {
            _webRequester = webRequester;
            _patientsResolverApiUrl = settings.Value.PatientsResolverApiUrl;
        }

        public async Task<HttpResponseMessage> Handle(GetPatientInfoQuery request, CancellationToken cancellationToken)
        {
            string url = $"{_patientsResolverApiUrl}/patientsApi/patients/{request.Organization}/{request.Id}";
            var responce = await _webRequester.SendRequest(url, "GET");
            return responce;
        }
    }
}
