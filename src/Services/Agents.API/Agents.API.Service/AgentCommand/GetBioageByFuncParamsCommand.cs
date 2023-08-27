using Agents.API.Entities;
using Agents.API.Entities.Mongo;
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
        private readonly PredictionRequestsService _pMService;
        private readonly PatientsService _requestService;

        //TODO продумать, как избавиться от такого.
        private readonly string _pressureDeltaParam = "PulsePressure";
        private readonly string _systolicPressure = "SystolicPressure";
        private readonly string _diastolicPressure = "DiastolicPressure";

        public ConcurrentDictionary<string, IProperty> Variables { get; set; }
        public ConcurrentDictionary<string, IProperty> Properties { get; set; }
        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }

        public GetBioageByFuncParamsCommand(PredictionRequestsService pMService,
            PatientsService requestService)
        {
            _pMService = pMService;
            _requestService = requestService;
        }

        public Delegate Command => async () =>
        {
            string patientId = Properties[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = Properties[PropertiesNamesSettings.Affiliation].Value as string;
            DateTime endTimestamp = (DateTime)Variables[PropertiesNamesSettings.EndTimestamp].Value;
            string mlModelId = (string)Variables[PropertiesNamesSettings.MlModel].Value;

            var meta = await _pMService.Get(mlModelId);
            if(meta == null)
                throw new ExecuteCommandException($"No meta for model id {mlModelId}");
            List<string> names = meta.ParamsNamesList;
            PatientParametersRequest request = new(patientAffiliation, patientId, endTimestamp, meta.ParamsNamesList);
            Dictionary<string, Parameter> parameters = await _requestService.GetPatientParameters(request);
            if(parameters == null)
                throw new ExecuteCommandException($"Cannot get latest parameters for patient {patientId}:{patientAffiliation}. See logs.");

            InitPressureDelta(parameters);

            double[] inputArgs = new double[names.Count];
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
                inputArgs[i] = parameters[names[i]].Value;      
            }

            var responce = await _pMService.Predict(mlModelId, inputArgs);
            if (responce == null)
                throw new ExecuteCommandException("Cannot get answer");

            if (!responce.IsSuccessStatusCode)
                throw new ExecuteCommandException($"{responce.StatusCode}:{responce.ReasonPhrase}");
            else
            {
                var res = await responce.DeserializeBody<float[]>();
                return (int)Math.Ceiling(res.First());
            }
        };

       

        //TODO придумать, как такое отслеживать.
        private void InitPressureDelta(Dictionary<string, Parameter> dict)
        {
            if (dict.ContainsKey(_pressureDeltaParam))
                return;
            if (!dict.ContainsKey(_systolicPressure) || !dict.ContainsKey(_diastolicPressure))
                return;
            dict[_pressureDeltaParam] = new Parameter()
            {
                Name = _pressureDeltaParam,
                Value = (dict[_diastolicPressure].Value - dict[_systolicPressure].Value)
            };
        }
    }
}
