using Interfaces;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Store
{
    public interface IParametersStore
    {
        public Task<IEnumerable<PatientParameter>> GetParameters(string patientId, DateTime start, DateTime end, List<string> names = null);

        public Task Insert(PatientParameter p);

        public Task Insert(IEnumerable<PatientParameter> parameters);

        public Task Delete(string id);

        public Task DeleteAll(string patientId);
    }
}
