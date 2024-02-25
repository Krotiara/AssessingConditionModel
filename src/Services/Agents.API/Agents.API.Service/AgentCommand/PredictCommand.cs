using Agents.API.Entities;
using Agents.API.Entities.Documents;
using Agents.API.Entities.Requests;
using Agents.API.Service.Services;
using Interfaces;
using ASMLib.DynamicAgent;
using Microsoft.Extensions.Logging;
using ASMLib.Entities;

namespace Agents.API.Service.AgentCommand
{
    public class PredictCommand : IAgentCommand
    {
        private readonly PredictionRequestsService _pMService;
        private readonly PatientsService _pPSerivce;
        private readonly ILogger<PredictCommand> _logger;
        private readonly string _ageParameter = "Age"; //TODO вынести в настройки. 

        public PredictCommand(PredictionRequestsService pMService, PatientsService requestService, ILogger<PredictCommand> logger)
        {
            _pMService = pMService;
            _pPSerivce = requestService;
            _logger = logger;
        }


        public IAgent Agent { get; set; }
        public AgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }

        public Delegate Command => async (double age, string mlModelId) =>
        {
            if (Agent == null || PropertiesNamesSettings == null)
                return new CommandResult($"Внутренняя ошибка выполнения команды.");

            var props = Agent.Properties;
            var vars = Agent.Variables;

            if (!CheckCommand())
                return new CommandResult($"No requered args in PredictCommand.");

            string patientId = props[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = props[PropertiesNamesSettings.Affiliation].Value as string;
            DateTime endTimestamp = (DateTime)vars[PropertiesNamesSettings.EndTimestamp].Value;

            var meta = await _pMService.Get(mlModelId);
            if (meta == null)
                return new CommandResult($"Не удалось получить информацию о модели прогноза {mlModelId}");

            PatientParametersRequest request = new(patientAffiliation, patientId, endTimestamp, meta.ParamsNamesList);
            Dictionary<string, Parameter> parameters = await _pPSerivce.GetPatientParameters(request);

            if (parameters == null)
                return new CommandResult($"Не удалось получить показатели пациента {patientId}:{patientAffiliation}.");

            double[] args = null;
            try
            {
                args = GetInputArgs(parameters, meta.ParamsNamesList, age);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return new CommandResult(ex.Message);
            }

            var responce = await _pMService.Predict(mlModelId, args);
            if (responce == null)
                return new CommandResult("Http request error.");
            else if (responce.Status == Entities.Requests.Responce.PredictStatus.WaitModelDownloading)
                return new CommandResult("Невозможно выполнить прогноз, т.к. модель прогноза еще загружается.");
            else if (responce.Status == Entities.Requests.Responce.PredictStatus.Error)
                return new CommandResult(responce.ErrorMessage);
            else
                return new CommandResult(responce.Predictions.First(), parameters.Values);
        };


        private bool CheckCommand() => Agent.Properties.ContainsKey(PropertiesNamesSettings.Id)
            && Agent.Properties.ContainsKey(PropertiesNamesSettings.Affiliation)
            && Agent.Variables.TryGetValue(PropertiesNamesSettings.EndTimestamp, out Property p)
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
                    throw new KeyNotFoundException($"Один из параметров не был найден: {names[i]}.");

                inputArgs[i] = parameters[names[i]].Value;
                Agent.AddToBuffer(parameters[names[i]]);
            }

            return inputArgs;
        }
    }
}
