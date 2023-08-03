using Agents.API.Entities;
using Agents.API.Entities.Mongo;
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
        private readonly PatientsRequestsService _requestService;

        public ConcurrentDictionary<string, IProperty> Variables { get; set; }
        public ConcurrentDictionary<string, IProperty> Properties { get; set; }

        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }


        public GetAgeCommand(PatientsRequestsService requestService)
        {
            _requestService = requestService;
        }

        public Delegate Command => async (DateTime timestamp) =>
        {
            string patientId = Properties[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = Properties[PropertiesNamesSettings.Affiliation].Value as string;

            //TODO избавиться от запроса пациента.
            var patient = await _requestService.GetPatientInfo(patientId, patientAffiliation);
            
            if(patient == null)
                throw new ExecuteCommandException($"No Patient {patientId}:{patientAffiliation}.");

            if (patient.Birthday == default(DateTime))
                throw new ExecuteCommandException($"No Birthday value for patient {patientId}:{patientAffiliation}.");

            if (timestamp < patient.Birthday)
                throw new ExecuteCommandException($"GetAgeCommand - timestamp is less than patient birthday " +
                    $"for patient {patientId}:{patientAffiliation}.");


            return GetAge(patient.Birthday, timestamp);
        };


        private int GetAge(DateTime dateOfBirth, DateTime dateOfMeasurement)
        {
            var a = ((dateOfMeasurement.Year * 100) + dateOfMeasurement.Month) * 100 + dateOfMeasurement.Day;
            var b = (((dateOfBirth.Year * 100) + dateOfBirth.Month) * 100) + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }
}
