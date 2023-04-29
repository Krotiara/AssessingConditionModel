using Agents.API.Entities;
using Agents.API.Entities.Requests;
using Agents.API.Service.Services;
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
        private readonly ModelKey _modelKey;
        private readonly PredcitionModelsService _pMService;
        private readonly PatientParametersService _pPSerivce;

        public GetBioageByFuncParamsCommand(PredcitionModelsService pMService, 
            PatientParametersService pPSerivce, IOptions<TempModelSettings> modelSets)
        {
            _pMService = pMService;
            _pPSerivce = pPSerivce;
            _modelKey = modelSets.Value.BioAge;
        }

        public Delegate Command => async (string patientId, string patientAffiliation, DateTime endTimestamp) =>
        {
#warning Нужно преобразовать везде int id to string id.
            int id = int.Parse(patientId);

            var meta = await _pMService.Get(_modelKey);
            if(meta == null)
                throw new ExecuteCommandException($"No meta for model key {_modelKey.Id}:{_modelKey.Version}");
            List<string> names = meta.ParamsNamesList;
            Dictionary<string, PatientParameter> parameters = await _pPSerivce.GetLatestParameters(new LatestParametersRequest()
            {
                PatientId = id,
                MedicalOrganization = patientAffiliation,
                EndTimestamp = endTimestamp,
                Names = names
            });
            if(parameters == null)
                throw new ExecuteCommandException($"Cannot get latest parameters for patient {patientId}:{patientAffiliation}. See logs.");
            float[] inputArgs = new float[names.Count];
            for(int i = 0; i < names.Count; i++)
            {
                if (parameters[names[i]] == null)
                    throw new ExecuteCommandException($"One of the required parameters is null: {names[i]}");
                inputArgs[i] = parameters[names[i]].ConvertValue<float>();
            }

            var responce = await _pMService.Predict(_modelKey, inputArgs);
            if (!responce.IsSuccessStatusCode)
                throw new ExecuteCommandException($"{responce.StatusCode}:{responce.ReasonPhrase}");
            else
            {
                var res = await responce.DeserializeBody<float[]>();
                return (int)Math.Ceiling(res.First());
            }

            //float[] inputArgs = new float[]
            //{
            //    pDict[ParameterNames.SystolicPressure].ConvertValue<float>(),
            //    pDict[ParameterNames.DiastolicPressure].ConvertValue<float>(),
            //    pDict[ParameterNames.SystolicPressure].ConvertValue<float>() - pDict[ParameterNames.DiastolicPressure].ConvertValue<float>(),
            //    pDict[ParameterNames.InhaleBreathHolding].ConvertValue<float>(),
            //    pDict[ParameterNames.OuthaleBreathHolding].ConvertValue<float>(),
            //    pDict[ParameterNames.LungCapacity].ConvertValue<float>(),
            //    pDict[ParameterNames.Weight].ConvertValue<float>(),
            //    pDict[ParameterNames.Accommodation].ConvertValue<float>(),
            //    pDict[ParameterNames.HearingAcuity].ConvertValue<float>(),
            //    pDict[ParameterNames.StaticBalancing].ConvertValue<float>()
            //};

            //IPredictRequest request = new PredictRequest() { Id = _modelKey.Id, Version = _modelKey.Version, Input = inputArgs };
            //string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            //string url = $"{_modelsServerUrl}/models/predict";

            //var responce = await _webRequester.SendRequest(url, "POST", requestBody);
            //if (!responce.IsSuccessStatusCode)
            //    throw new ExecuteCommandException($"{responce.StatusCode}:{responce.ReasonPhrase}");
            //else
            //{
            //    var res = await _webRequester.DeserializeBody<float[]>(responce);
            //    return (int)Math.Ceiling(res.First());
            //}
        };
    }
}
