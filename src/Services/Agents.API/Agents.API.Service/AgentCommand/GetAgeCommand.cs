using Agents.API.Entities;
using Agents.API.Service.Services;
using Interfaces;
using ASMLib.DynamicAgent;
using ASMLib.Entities;

namespace Agents.API.Service.AgentCommand
{

    public class GetAgeCommand : IAgentCommand
    {
        private readonly PatientsService _requestService;

        public IAgent Agent { get; set; }

        public AgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }


        public GetAgeCommand(PatientsService requestService)
        {
            _requestService = requestService;
        }

        public Delegate Command => async (DateTime timestamp) =>
        {
            if (Agent == null || PropertiesNamesSettings == null)
                return new CommandResult($"Внутренняя ошибка выполнения команды.");

            if (!Agent.Properties.TryGetValue(PropertiesNamesSettings.Id, out var idProp) ||
                !Agent.Properties.TryGetValue(PropertiesNamesSettings.Affiliation, out var affiliationProp))
                return new CommandResult($"Не удалось получить информацию о пациенте: не передан идентификатор.");

            string patientId = idProp.Value as string;
            string patientAffiliation = affiliationProp.Value as string;

            //TODO избавиться от запроса пациента.
            var patient = await _requestService.GetPatientInfo(patientId, patientAffiliation, false);

            if (patient == null)
                return new CommandResult($"Не удалось получить информацию о пациенте {patientId}:{patientAffiliation}.");

            if (patient.Birthday == default(DateTime))
                return new CommandResult($"Не установлена дата рождения у пациента {patientId}:{patientAffiliation}.");

            if (timestamp < patient.Birthday)
                return new CommandResult($"Указанная дата прогноза меньше даты рождения пациента {patientId}:{patientAffiliation}.");

            double age = GetAge((DateTime)patient.Birthday, timestamp);
            return new CommandResult(age);
        };


        private double GetAge(DateTime dateOfBirth, DateTime dateOfMeasurement)
        {
            var a = ((dateOfMeasurement.Year * 100) + dateOfMeasurement.Month) * 100 + dateOfMeasurement.Day;
            var b = (((dateOfBirth.Year * 100) + dateOfBirth.Month) * 100) + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }
}
