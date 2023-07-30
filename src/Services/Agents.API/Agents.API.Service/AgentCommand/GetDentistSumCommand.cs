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
        private readonly PatientsRequestsService _pPSerivce;
        private readonly string _modelsServerUrl;
        private readonly EnvSettings _settings;
        private readonly TempModelSettings _modelSets;

        private readonly string _ageParameter = "Age"; //TODO вынести в настройки. 

        public ConcurrentDictionary<string, IProperty> Variables { get; set; }
        public ConcurrentDictionary<string, IProperty> Properties { get; set; }
        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }


        public GetDentistSumCommand(PredictionRequestsService pMService,
            PatientsRequestsService pPSerivce, 
            IOptions<EnvSettings> settings, 
            IOptions<TempModelSettings> modelSets)
        {
            _pMService = pMService;
            _pPSerivce = pPSerivce;
            _modelsServerUrl = settings.Value.ModelsApiUrl;
            _settings = settings.Value;
            _modelSets = modelSets.Value;
        }

        public Delegate Command => async (int age) =>
        {
            string patientId = Properties[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = Properties[PropertiesNamesSettings.Affiliation].Value as string;
            DateTime endTimestamp = (DateTime)Variables[PropertiesNamesSettings.EndTimestamp].Value;

#warning Костыльное получение версии и Id.
            ModelKey model = GetModelByAge(age);
            if (model == null)
                throw new ExecuteCommandException($"Cannot resolve model key");

            var meta = await _pMService.Get(model);
            if (meta == null)
                throw new ExecuteCommandException($"No meta for model key {model.Id}:{model.Version}");
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

            var responce = await _pMService.Predict(model, inputArgs);
            if (!responce.IsSuccessStatusCode)
                throw new ExecuteCommandException($"{responce.StatusCode}:{responce.ReasonPhrase}");
            else
            {
                var res = await responce.DeserializeBody<float[]>();
                return (int)Math.Ceiling(res.First());
            }
        };

       
        private ModelKey GetModelByAge(float age)
        {
            if (age >= 3 && age <= 5)
                return _modelSets.Dentist_3_5;
            if (age >= 6 && age <= 9)
                return _modelSets.Dentist_6_9;
            else if (age >= 9 && age <= 12)
                return _modelSets.Dentist_10_12;
            else return null;
        }
    }
}
