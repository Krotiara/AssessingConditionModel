using Agents.API.Entities;
using Agents.API.Entities.Requests;
using Agents.API.Service.Services;
using Interfaces;
using Interfaces.DynamicAgent;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
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

        //TODO продумать, как избавиться от такого.
        private readonly string _pressureDeltaParam = "PulsePressure";
        private readonly string _systolicPressure = "SystolicPressure";
        private readonly string _diastolicPressure = "DiastolicPressure";

        public ConcurrentDictionary<string, IProperty> Variables { get; set; }
        public ConcurrentDictionary<string, IProperty> Properties { get; set; }
        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }

        public GetBioageByFuncParamsCommand(PredcitionModelsService pMService, 
            PatientParametersService pPSerivce, IOptions<TempModelSettings> modelSets)
        {
            _pMService = pMService;
            _pPSerivce = pPSerivce;
            _modelKey = modelSets.Value.BioAge;
        }

        public Delegate Command => async () =>
        {
            string patientId = Properties[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = Properties[PropertiesNamesSettings.Affiliation].Value as string;
            DateTime endTimestamp = (DateTime)Variables[PropertiesNamesSettings.EndTimestamp].Value;

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

            InitPressureDelta(parameters);

            float[] inputArgs = new float[names.Count];
            for(int i = 0; i < names.Count; i++)
            {
                if (Variables.ContainsKey(names[i]) && Variables[names[i]].Value != null)
                {
                    inputArgs[i] = Variables[names[i]].ConvertValue<float>();
                    continue;
                }

                if (Properties.ContainsKey(names[i]) && Properties[names[i]].Value != null)
                {
                    inputArgs[i] = Properties[names[i]].ConvertValue<float>();
                    continue;
                }

                if (!parameters.ContainsKey(names[i]))
                    throw new ExecuteCommandException($"One of the required parameters is not found: {names[i]}");
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
        };

       

        //TODO придумать, как такое отслеживать.
        private void InitPressureDelta(Dictionary<string, PatientParameter> dict)
        {
            if (dict.ContainsKey(_pressureDeltaParam))
                return;
            if (!dict.ContainsKey(_systolicPressure) || !dict.ContainsKey(_diastolicPressure))
                return;
            dict[_pressureDeltaParam] = new PatientParameter()
            {
                Name = _pressureDeltaParam,
                Value = (dict[_diastolicPressure].ConvertValue<float>() - dict[_systolicPressure].ConvertValue<float>()).ToString()
            };
        }
    }
}
