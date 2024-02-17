using Interfaces;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Store
{
    public interface IParametersStore<T> where T : IPatientParameter
    {
        public Task<IEnumerable<IPatientParameter>> GetParameters(string patientId, DateTime start, DateTime end, List<string> names = null);

        public Task Insert(string patientId, IPatientParameter p);

        public Task Insert(string patientId, IEnumerable<IPatientParameter> parameters);

        public Task Delete(string id);

        public Task DeleteAll(string patientId);
    }
}
