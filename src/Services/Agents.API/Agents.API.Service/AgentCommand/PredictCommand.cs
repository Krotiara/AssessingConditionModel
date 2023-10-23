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


        public IAgent Agent { get; set; }
        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }

        public Delegate Command => async (double age, string mlModelId) =>
        {
            var props = Agent.Properties;
            var vars = Agent.Variables;

            if (!CheckCommand())
                throw new ExecuteCommandException($"No requered args in PredictCommand.");

            string patientId = props[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = props[PropertiesNamesSettings.Affiliation].Value as string;
            DateTime endTimestamp = (DateTime)vars[PropertiesNamesSettings.EndTimestamp].Value;

            var meta = await _pMService.Get(mlModelId);
            if (meta == null)
                throw new ExecuteCommandException($"No meta for model id {mlModelId}");

            PatientParametersRequest request = new(patientAffiliation, patientId, endTimestamp, meta.ParamsNamesList);
            Dictionary<string, Parameter> parameters = await _pPSerivce.GetPatientParameters(request);

            if (parameters == null)
                throw new ExecuteCommandException($"Cannot get latest parameters for patient {patientId}:{patientAffiliation}.");

            FillBuffer(parameters.Values);

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


        private void FillBuffer(IEnumerable<Parameter> patientParameters)
        {
            foreach (var parameter in patientParameters)
                Agent.Buffer[(parameter.Name, parameter.Timestamp)] = parameter;
        }


        private bool CheckCommand() => Agent.Properties.ContainsKey(PropertiesNamesSettings.Id)
            && Agent.Properties.ContainsKey(PropertiesNamesSettings.Affiliation)
            && Agent.Variables.TryGetValue(PropertiesNamesSettings.EndTimestamp, out IProperty p)
            && p.Value is DateTime;


        private double[] GetInputArgs(Dictionary<string, Parameter> parameters, List<string> names, double age)
        {
            var props = Agent.Properties;
            var vars = Agent.Variables;

            double[] inputArgs = new double[names.Count];

            for (int i = 0; i < names.Count; i++)
            {
                if (vars.ContainsKey(names[i]) && vars[names[i]].Value != null)
                {
                    inputArgs[i] = vars[names[i]].ConvertValue<float>();
                    continue;
                }

                if (props.ContainsKey(names[i]) && props[names[i]].Value != null)
                {
                    inputArgs[i] = props[names[i]].ConvertValue<float>();
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
