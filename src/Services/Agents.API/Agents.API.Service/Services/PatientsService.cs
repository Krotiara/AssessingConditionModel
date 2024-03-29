﻿using Agents.API.Entities;
using Agents.API.Entities.Documents;
using Agents.API.Entities.Requests;
using ASMLib.Entities;
using Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class PatientsService
    {
        private readonly IWebRequester _webRequester;
        private readonly string _patientsResolverApiUrl;
        private readonly ILogger<PatientsService> _logger;

        private readonly ConcurrentDictionary<(string, string), PatientInfo> _patients;

        public PatientsService(IWebRequester webRequester, IOptions<EnvSettings> settings, ILogger<PatientsService> logger)
        {
            _webRequester = webRequester;
            _patientsResolverApiUrl = $"{settings.Value.PatientsResolverApiUrl}/patientsApi";
            _logger = logger;
            _patients = new();
        }


        public async Task<PatientInfo?> GetPatientInfo(string id, string affiliation, bool isRefresh)
        {
            if (isRefresh && _patients.TryGetValue((id, affiliation), out PatientInfo p))
                return p;


            string url = $"{_patientsResolverApiUrl}/Patients/patient";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(new GetPatientRequest() { PatientId = id, Affiliation = affiliation });
            var responce = await _webRequester.SendRequest(url, "POST", body);
            if (responce != null && responce.IsSuccessStatusCode)
            {
                p = await responce.DeserializeBody<PatientInfo>();
                _patients[(id, affiliation)] = p;
                return p;
            }
            else
            {
                _logger.LogError($"Cannot get patient info {id}({affiliation}).");
                return null;
            }
        }


        public async Task<IList<Influence>> GetInfluences(PatientInfluencesRequest request)
        {
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            string url = $"{_patientsResolverApiUrl}/Influences/influences";
            var responce = await _webRequester.SendRequest(url, "POST", body);
            if (responce != null && responce.IsSuccessStatusCode)
                return await responce.DeserializeBody<List<Influence>>();
            else
            {
                _logger.LogError($"Cannot get patient influences.");
                return null;
            }
        }


        public async Task<Dictionary<string, Parameter>> GetPatientParameters(PatientParametersRequest request)
        {
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            //TODO Запрос latestParameters
            string url = $"{_patientsResolverApiUrl}/Patients/parameters";
            var responce = await _webRequester.SendRequest(url, "POST", body);
            if (responce == null || !responce.IsSuccessStatusCode)
            {
                _logger.LogError($"Cannot get latest parameters by request: no responce or responce is bad.");
                return null;
            }
            var res = await responce.DeserializeBody<List<Parameter>>();
            var resDict = new Dictionary<string, Parameter>();
            foreach (var p in res)
            {
                if (resDict.TryGetValue(p.Name, out Parameter resP) && p.Timestamp > resP.Timestamp)
                {
                    resDict[p.Name] = p;
                    continue;
                }
                resDict[p.Name] = p;
            }
            return resDict;
        }
    }
}
