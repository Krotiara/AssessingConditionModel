using Agents.API.Entities;
using Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{
    public class GetBioageByFuncParamsCommand : IAgentCommand
    {
        private readonly IWebRequester _webRequester;
        private readonly string _modelsServerUrl;

        private readonly ModelKey _modelKey;

        public GetBioageByFuncParamsCommand(IWebRequester webRequester, 
            IOptions<EnvSettings> settings, IOptions<TempModelSettings> modelSets)
        {
            _webRequester = webRequester;
            _modelsServerUrl = settings.Value.ModelsApiUrl;
            _modelKey = modelSets.Value.BioAge;
        }

        public Delegate Command => async (Dictionary<ParameterNames, PatientParameter> pDict) =>
        {
            float[] inputArgs = new float[]
            {
                pDict[ParameterNames.SystolicPressure].ConvertValue<float>(),
                pDict[ParameterNames.DiastolicPressure].ConvertValue<float>(),
                pDict[ParameterNames.SystolicPressure].ConvertValue<float>() - pDict[ParameterNames.DiastolicPressure].ConvertValue<float>(),
                pDict[ParameterNames.InhaleBreathHolding].ConvertValue<float>(),
                pDict[ParameterNames.OuthaleBreathHolding].ConvertValue<float>(),
                pDict[ParameterNames.LungCapacity].ConvertValue<float>(),
                pDict[ParameterNames.Weight].ConvertValue<float>(),
                pDict[ParameterNames.Accommodation].ConvertValue<float>(),
                pDict[ParameterNames.HearingAcuity].ConvertValue<float>(),
                pDict[ParameterNames.StaticBalancing].ConvertValue<float>()
            };

            IPredictRequest request = new PredictRequest() { Id = _modelKey.Id, Version = _modelKey.Version, Input = inputArgs };
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            string url = $"{_modelsServerUrl}/models/predict/";

            var responce = await _webRequester.SendRequest(url, "POST", requestBody);
            if (!responce.IsSuccessStatusCode)
                throw new ExecuteCommandException($"{responce.StatusCode}:{responce.ReasonPhrase}");
            else
            {
                var res = await _webRequester.DeserializeBody<float[]>(responce);
                return (int)Math.Ceiling(res.First());
            }
        };
    }
}
