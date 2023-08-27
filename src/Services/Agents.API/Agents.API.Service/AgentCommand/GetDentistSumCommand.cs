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
    public class GetDentistSumCommand : IAgentCommand
    {
        private readonly PredictionRequestsService _pMService;
        private readonly PatientsService _pPSerivce;
        private readonly string _modelsServerUrl;
        private readonly EnvSettings _settings;

        private readonly string _ageParameter = "Age"; //TODO вынести в настройки. 

        public ConcurrentDictionary<string, IProperty> Variables { get; set; }
        public ConcurrentDictionary<string, IProperty> Properties { get; set; }
        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }


        public GetDentistSumCommand(PredictionRequestsService pMService,
            PatientsService pPSerivce, 
            IOptions<EnvSettings> settings)
        {
            _pMService = pMService;
            _pPSerivce = pPSerivce;
            _modelsServerUrl = settings.Value.ModelsApiUrl;
            _settings = settings.Value;
        }

        public Delegate Command => async (int age) =>
        {
            string patientId = Properties[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = Properties[PropertiesNamesSettings.Affiliation].Value as string;
            DateTime endTimestamp = (DateTime)Variables[PropertiesNamesSettings.EndTimestamp].Value;
            string mlModelId = (string)Variables[PropertiesNamesSettings.MlModel].Value;

            if (mlModelId == null)
                throw new ExecuteCommandException($"Cannot resolve model key");

            var meta = await _pMService.Get(mlModelId);
            if (meta == null)
                throw new ExecuteCommandException($"No meta for model id {mlModelId}");
            List<string> names = meta.ParamsNamesList;
            PatientParametersRequest request = new(patientAffiliation, patientId, endTimestamp, names);
            Dictionary<string, Parameter> parameters = await _pPSerivce.GetPatientParameters(request);

            if (parameters == null)
                throw new ExecuteCommandException($"Cannot get latest parameters for patient {patientId}:{patientAffiliation}. See logs.");

            double[] inputArgs = new double[names.Count];
            for (int i = 0; i < names.Count; i++)
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

                if (names[i] == _ageParameter)
                {
                    inputArgs[i] = age;
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
    }
}
