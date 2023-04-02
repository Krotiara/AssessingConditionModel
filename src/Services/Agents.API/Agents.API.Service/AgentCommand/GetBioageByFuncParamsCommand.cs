using Agents.API.Entities;
using Interfaces;
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

        public GetBioageByFuncParamsCommand(IWebRequester webRequester, EnvSettings settings)
        {
            _webRequester = webRequester;
            _modelsServerUrl = settings.ModelsApiUrl;
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
            IPredictRequest request = new PredictRequest() { ModelId = "bioAgeFuncModel", InputArgs = inputArgs };
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            string url = $"{_modelsServerUrl}/models/predict/";
            float[] res = (await _webRequester.GetResponse<float[]>(url, "POST", requestBody));
            return (int)Math.Ceiling(res.First());
        };
    }
}
