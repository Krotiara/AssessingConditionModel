﻿using Agents.API.Entities;
using Agents.API.Entities.Requests;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{
    public class GetLatestPatientParametersCommand : IAgentCommand
    {
        private readonly IWebRequester _webRequester;
        private readonly string _patientsResolverApiUrl;

        public GetLatestPatientParametersCommand(IWebRequester webRequester, EnvSettings settings)
        {
            _webRequester = webRequester;
            _patientsResolverApiUrl = settings.PatientsResolverApiUrl;
        }

        public Delegate Command => async (DateTime startTimestamp, DateTime endTimestamp, int patientId, string medOrganization) =>
        {
            PatientParametersRequest request = new PatientParametersRequest()
            {
                PatientId = patientId,
                MedicalOrganization = medOrganization,
                StartTimestamp = startTimestamp,
                EndTimestamp = endTimestamp
            };
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            string url = $"{_patientsResolverApiUrl}/patientsApi/latestPatientParameters";

            var responce = await _webRequester.SendRequest(url, "POST", body);
            if (!responce.IsSuccessStatusCode)
                throw new ExecuteCommandException($"{responce.StatusCode}:{responce.ReasonPhrase}");
            else
            {
                var res = await _webRequester.DeserializeBody<IList<PatientParameter>>(responce);
                return res.ToDictionary(x => x.ParameterName, x => x);
            }
        };
    }
}
