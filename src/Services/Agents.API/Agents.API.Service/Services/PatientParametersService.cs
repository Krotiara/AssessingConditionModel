using Agents.API.Entities;
using Agents.API.Entities.Requests;
using Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Agents.API.Service.Services
{
    public class PatientParametersService
    {
        private readonly IWebRequester _webRequester;
        private readonly string _patientsResolverApiUrl;
        private readonly ILogger<PatientParametersService> _logger;

        public PatientParametersService(IWebRequester webRequester, IOptions<EnvSettings> settings, ILogger<PatientParametersService> logger)
        {
            _webRequester = webRequester;
            _patientsResolverApiUrl = settings.Value.PatientsResolverApiUrl;
            _logger = logger;
        }


        public async Task<Dictionary<string, PatientParameter>> GetPatientParameters(PatientParametersRequest request)
        {
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            //TODO Запрос latestParameters
            string url = $"{_patientsResolverApiUrl}/patientsApi/Patients/parameters";
            var responce = await _webRequester.SendRequest(url, "POST", body);
            if(!responce.IsSuccessStatusCode)
            {
                _logger.LogError($"Cannot get latest parameters by request: {responce.StatusCode}.");
                return null;
            }
            var res = await responce.DeserializeBody<List<PatientParameter>>();
            return res.ToDictionary(x=>x.Name, x=>x);
        }
    }
}
