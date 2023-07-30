using Agents.API.Entities;
using Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class RequestService
    {
        private readonly IWebRequester _webRequester;
        private readonly string _patientsResolverApiUrl;

        public RequestService(IWebRequester webRequester, IOptions<EnvSettings> settings)
        {
            _webRequester = webRequester;
            _patientsResolverApiUrl = settings.Value.PatientsResolverApiUrl;
        }


        public async Task<HttpResponseMessage> GetPatientInfo(string id, string affiliation)
        {
            string url = $"{_patientsResolverApiUrl}/patientsApi/patients/{affiliation}/{id}";
            var responce = await _webRequester.SendRequest(url, "GET");
            return responce;
        }
    }


    
}
