using Agents.API.Entities;
using Agents.API.Entities.Documents;
using Agents.API.Entities.Requests;
using Agents.API.Service.Services;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{
    public class PredictCommand : IAgentCommand
    {
        private readonly PredictionRequestsService _pMService;
        private readonly PatientsService _pPSerivce;
        private readonly string _ageParameter = "Age"; //TODO вынести в настройки. 

        public PredictCommand(PredictionRequestsService pMService, PatientsService requestService)
        {
            _pMService = pMService;
            _pPSerivce = requestService;
        }

        public ConcurrentDictionary<string, IProperty> Variables { get; set; }
        public ConcurrentDictionary<string, IProperty> Properties { get; set; }
        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }

        public Delegate Command => async (double age, string mlModelId) =>
        {
            if (!CheckCommand())
                throw new ExecuteCommandException($"No requered args in PredictCommand.");

            string patientId = Properties[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = Properties[PropertiesNamesSettings.Affiliation].Value as string;
            DateTime endTimestamp = (DateTime)Variables[PropertiesNamesSettings.EndTimestamp].Value;

            var meta = await _pMService.Get(mlModelId);
            if (meta == null)
                throw new ExecuteCommandException($"No meta for model id {mlModelId}");

            PatientParametersRequest request = new(patientAffiliation, patientId, endTimestamp, meta.ParamsNamesList);
            Dictionary<string, Parameter> parameters = await _pPSerivce.GetPatientParameters(request);

            if (parameters == null)
                throw new ExecuteCommandException($"Cannot get latest parameters for patient {patientId}:{patientAffiliation}.");

            double[] args = GetInputArgs(parameters, meta.ParamsNamesList, age);

            var responce = await _pMService.Predict(mlModelId, args);
            if (responce == null)
                throw new ExecuteCommandException("Cannot get answer");

            if (!responce.IsSuccessStatusCode)
                throw new ExecuteCommandException($"{responce.StatusCode}:{responce.ReasonPhrase}");
            else
            {
                var res = await responce.DeserializeBody<float[]>();
                return res.First(); //TODO - убрать first
            }
        };


        private bool CheckCommand() => Properties.ContainsKey(PropertiesNamesSettings.Id)
            && Properties.ContainsKey(PropertiesNamesSettings.Affiliation)
            && Variables.TryGetValue(PropertiesNamesSettings.EndTimestamp, out IProperty p)
            && p.Value is DateTime;


        private double[] GetInputArgs(Dictionary<string, Parameter> parameters, List<string> names, double age)
        {
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

            return inputArgs;
        }
    }
}
