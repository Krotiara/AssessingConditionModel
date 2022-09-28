using MediatR;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Query
{
    public class GetPatientInfluencesQuery: IRequest<List<Influence>>
    {
        public int PatientId { get; }

        public DateTime StartTimestamp { get; }

        public DateTime EndTimestamp { get; }

        public GetPatientInfluencesQuery(int patientId, DateTime start, DateTime end)
        {
            PatientId = patientId;
            StartTimestamp = start;
            EndTimestamp = end;
        }
    }
}
