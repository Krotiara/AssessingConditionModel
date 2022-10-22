using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempGateway.Service.Service
{
    public interface IPatientService
    {
        public Task<IPatient> GetPatientById(int id);

        public Task<IAgingPatientState> GetAgingPatientStateByPatientId(int patientId);

        public Task<IList<IAgingPatientState>> GetAgingDynamicsByPatientId(int patientId);
    }
}
