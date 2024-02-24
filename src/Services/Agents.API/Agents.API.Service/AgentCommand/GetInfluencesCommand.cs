using Agents.API.Entities;
using Agents.API.Entities.Requests;
using Agents.API.Service.Services;
using Interfaces;
using ASMLib.DynamicAgent;

namespace Agents.API.Service.AgentCommand
{
    public class GetInfluencesCommand : IAgentCommand
    {
        private readonly PatientsService _requestService;

        public IAgent Agent { get; set; }
        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }

        public GetInfluencesCommand(PatientsService requestService)
        {
            _requestService = requestService;
        }

        public Delegate Command => async () =>
        {
            if (PropertiesNamesSettings == null || Agent == null)
                return new CommandResult($"Не удалось получить воздействия на пациента.");
            if (!Agent.Properties.TryGetValue(PropertiesNamesSettings.Id, out var idProp) ||
            !Agent.Properties.TryGetValue(PropertiesNamesSettings.Affiliation, out var affiliationProp) ||
            !Agent.Variables.TryGetValue(PropertiesNamesSettings.StartTimestamp, out var startProp) ||
            !Agent.Variables.TryGetValue(PropertiesNamesSettings.EndTimestamp, out var endProp))
                return new CommandResult($"Не удалось получить воздействия на пациента: недостаточно данных для поиска).");

            string patientId = idProp.Value as string;
            string patientAffiliation = affiliationProp.Value as string;
            DateTime startTimestamp = (DateTime)startProp.Value;
            DateTime endTimestamp = (DateTime)endProp.Value;


            PatientInfluencesRequest request = new()
            {
                PatientId = patientId,
                Affiliation = patientAffiliation,
                StartTimestamp = startTimestamp,
                EndTimestamp = endTimestamp
            };

            var influences = await _requestService.GetInfluences(request);

            if (influences == null)
                return new CommandResult($"Не удалось получить воздействия на пациента {request.PatientId}({request.Affiliation}).");

            return new CommandResult(influences);
        };


    }
}
