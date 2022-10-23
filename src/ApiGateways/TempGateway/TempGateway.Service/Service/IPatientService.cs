using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempGateway.Entities;

namespace TempGateway.Service.Service
{
    public interface IPatientService
    {
        public Task<IPatient> GetPatientById(int id);

        public Task<IAgingPatientState> GetAgingPatientStateByPatientId(int patientId);

        public Task<IList<AgingPatientState>> GetAgingDynamicsByPatientId(int patientId, DateTime startTimestamp, DateTime endTimestamp);
    }
}
