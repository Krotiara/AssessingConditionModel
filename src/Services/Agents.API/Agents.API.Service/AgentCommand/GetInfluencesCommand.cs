﻿using Agents.API.Entities;
using Agents.API.Entities.Requests;
using Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{
    public class GetInfluencesCommand : IAgentCommand
    {
        private readonly IWebRequester _webRequester;
        private readonly string _patientsResolverApiUrl;

        public GetInfluencesCommand(IWebRequester webRequester, IOptions<EnvSettings> settings)
        {
            _webRequester = webRequester;
            _patientsResolverApiUrl = settings.Value.PatientsResolverApiUrl;
        }

        public Delegate Command => async (DateTime startTimestamp, DateTime endTimestamp, string patientId, string medOrganization) =>
        {
#warning patientId нужно полность преобразовать в string на бэкэ
            PatientInfluencesRequest request = new()
            {
                PatientId = int.Parse(patientId),
                MedicalOrganization = medOrganization,
                StartTimestamp = startTimestamp,
                EndTimestamp = endTimestamp
            };
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            string url = $"{_patientsResolverApiUrl}/patientsApi/influences";
            var responce = await _webRequester.SendRequest(url, "POST", body);
            if (!responce.IsSuccessStatusCode)
                throw new ExecuteCommandException($"{responce.StatusCode}:{responce.ReasonPhrase}");
            else
            {
                var res = await _webRequester.DeserializeBody<IList<Influence>>(responce);
                return res;
            }
        };
    }
}
