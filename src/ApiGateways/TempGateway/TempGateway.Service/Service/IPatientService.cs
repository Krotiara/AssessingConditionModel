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

        public Task<IAgingState> GetAgingPatientStateByPatientId(int patientId);

        public Task<IList<AgingDynamics>> GetAgingDynamicsByPatientId(int patientId, DateTime startTimestamp, DateTime endTimestamp);

        public Task<IList<AgingDynamics>> GetAgingDynamics(DateTime startTimestamp, DateTime endTimestamp);
    }
}
