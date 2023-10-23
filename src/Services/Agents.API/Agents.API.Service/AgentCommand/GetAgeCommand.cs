using Agents.API.Entities;
using Agents.API.Entities.Documents;
using Agents.API.Service.Services;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{

    public class GetAgeCommand : IAgentCommand
    {
        private readonly PatientsService _requestService;

        public IAgent Agent { get; set; }

        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }


        public GetAgeCommand(PatientsService requestService)
        {
            _requestService = requestService;
        }

        public Delegate Command => async (DateTime timestamp) =>
        {
            string patientId = Agent.Properties[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = Agent.Properties[PropertiesNamesSettings.Affiliation].Value as string;

            //TODO избавиться от запроса пациента.
            var patient = await _requestService.GetPatientInfo(patientId, patientAffiliation, false);
            
            if(patient == null)
                throw new ExecuteCommandException($"No Patient {patientId}:{patientAffiliation}.");

            if (patient.Birthday == default(DateTime))
                throw new ExecuteCommandException($"No Birthday value for patient {patientId}:{patientAffiliation}.");

            if (timestamp < patient.Birthday)
                throw new ExecuteCommandException($"GetAgeCommand - timestamp is less than patient birthday " +
                    $"for patient {patientId}:{patientAffiliation}.");


            return GetAge(patient.Birthday, timestamp);
        };


        private double GetAge(DateTime dateOfBirth, DateTime dateOfMeasurement)
        {
            var a = ((dateOfMeasurement.Year * 100) + dateOfMeasurement.Month) * 100 + dateOfMeasurement.Day;
            var b = (((dateOfBirth.Year * 100) + dateOfBirth.Month) * 100) + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }
}
