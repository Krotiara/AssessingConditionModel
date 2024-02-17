using Interfaces;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Store
{
    public class PostgreSQLParametersStore : IParametersStore<PatientParameter>
    {
        private readonly PostgreSQLParametersDbContext _dbContext;

        public PostgreSQLParametersStore(PostgreSQLParametersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAll(string patientId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IPatientParameter>> GetParameters(string patientId, DateTime start, DateTime end, List<string> names = null)
        {
            throw new NotImplementedException();
        }

        public Task Insert(string patientId, IPatientParameter p)
        {
            throw new NotImplementedException();
        }

        public Task Insert(string patientId, IEnumerable<IPatientParameter> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
