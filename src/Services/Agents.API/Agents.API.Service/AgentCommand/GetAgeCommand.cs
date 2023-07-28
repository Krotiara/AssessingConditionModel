using Agents.API.Entities;
using Agents.API.Entities.Mongo;
using Agents.API.Service.Query;
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
    //TODO преобразовать в получение по дате рождения и переданному времени.
    public class GetAgeCommand : IAgentCommand
    {
        private readonly IMediator _mediator;

        
        public ConcurrentDictionary<string, IProperty> Variables { get; set; }
        public ConcurrentDictionary<string, IProperty> Properties { get; set; }

        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }


        public GetAgeCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Delegate Command => async (DateTime timestamp) =>
        {
            string patientId = Properties[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = Properties[PropertiesNamesSettings.Affiliation].Value as string;

            //TODO избавиться от запроса пациента.
            var responce = await _mediator.Send(new GetPatientInfoQuery() { Id = patientId, Organization = patientAffiliation });
            if (!responce.IsSuccessStatusCode)
                throw new ExecuteCommandException($"Cannot get patient {patientId}:{patientAffiliation} info: " +
                    $"{responce.StatusCode}:{responce.ReasonPhrase}");
            var patient = await responce.DeserializeBody<Patient>();
            if (patient.Birthday == default(DateTime))
                throw new ExecuteCommandException($"No Birthday value for patient {patientId}:{patientAffiliation}.");

            if(timestamp < patient.Birthday)
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
