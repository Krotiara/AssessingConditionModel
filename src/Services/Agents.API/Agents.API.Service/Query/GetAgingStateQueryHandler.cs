using Agents.API.Data.Repository;
using Agents.API.Entities;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetAgingStateQueryHandler : IRequestHandler<GetAgingStateQuery, AgingState>
    {
        private readonly IDynamicAgentsRepository agentPatientsRepository;

        public GetAgingStateQueryHandler(IDynamicAgentsRepository agentPatientsRepository)
        {
            this.agentPatientsRepository = agentPatientsRepository;
        }

        public async Task<AgingState> Handle(GetAgingStateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IDynamicAgent agentPatient = agentPatientsRepository.GetAgent(request.PatientId, AgentType.AgingPatient);
                if (agentPatient == null)
                    throw new GetAgingStateException($"Agent patient for patient with id = {request.PatientId} not found.");

                agentPatient.Settings.ActionsArgsReplaceDict[CommonArgs.EndDateTime] = request.Timestamp;
                agentPatient.Settings.ActionsArgsReplaceDict[CommonArgs.StartDateTime] = DateTime.MinValue; //TODO - по идее лучше так не делать, так как захватывает все данные из бд от начала до timeStamp.
                await agentPatient.UpdateState();

                long age = agentPatient.Settings.GetPropertyValue<long>("CurrentAge");
                long bioAge = agentPatient.Settings.GetPropertyValue<long>("CurrentBioAge");
                AgentBioAgeStates agingRang = agentPatient.Settings.GetPropertyValue<AgentBioAgeStates>("CurrentAgeRang");

                AgingState state = new AgingState()
                {
                    PatientId = request.PatientId,
                    Age = age,
                    BioAge = bioAge,
                    BioAgeState = agingRang,
                    Timestamp = DateTime.Today //TODO переименовать query под currentState
                };
                return state;
            }
            catch(AgentNotFoundException ex)
            {
                throw new GetAgingStateException("Agent was not found", ex);
            }
        }
    }
}
