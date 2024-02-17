using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Data.Store
{
    public class PostgreSQLParametersStore : IParametersStore
    {
        private readonly PostgreSQLParametersDbContext _dbContext;

        public PostgreSQLParametersStore(PostgreSQLParametersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(string id)
        {
            var p = _dbContext.PatientParameters.SingleOrDefault(x => x.Id == id);
            if (p != null)
            {
                _dbContext.PatientParameters.Remove(p);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAll(string patientId)
        {
            var parameters = _dbContext.PatientParameters.AsQueryable().Where(x => x.Id == patientId);
            foreach (var p in parameters)
                _dbContext.PatientParameters.Remove(p);
            await _dbContext.SaveChangesAsync();
        }

        public Task<IEnumerable<PatientParameter>> GetParameters(string patientId, DateTime start, DateTime end, List<string> names = null)
        {
            throw new NotImplementedException();
        }

        public async Task Insert(PatientParameter p)
        {
            if (p.Id == null)
                await _dbContext.PatientParameters.AddAsync(p);
            else
            {
                var dbParam = _dbContext.PatientParameters.SingleOrDefault(x => x.Id == p.Id);
                if (dbParam == null)
                    await _dbContext.PatientParameters.AddAsync(p);
                else
                {
                    dbParam.Name = p.Name;
                    dbParam.Value = p.Value;
                    dbParam.Timestamp = p.Timestamp;
                    dbParam.PatientId = p.PatientId;
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task Insert(IEnumerable<PatientParameter> parameters)
        {
            foreach (var p in parameters)
            {
                if (p.Id == null)
                    await _dbContext.PatientParameters.AddAsync(p);
                else
                {
                    var dbParam = _dbContext.PatientParameters.SingleOrDefault(x => x.Id == p.Id);
                    if (dbParam == null)
                        await _dbContext.PatientParameters.AddAsync(p);
                    else
                    {
                        dbParam.Name = p.Name;
                        dbParam.Value = p.Value;
                        dbParam.Timestamp = p.Timestamp;
                        dbParam.PatientId = p.PatientId;
                    }
                }


            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
