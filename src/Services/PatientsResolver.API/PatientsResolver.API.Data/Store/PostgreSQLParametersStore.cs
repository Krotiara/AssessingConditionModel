using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Data.Store
{
    public class PostgreSQLParametersStore : IParametersStore
    {
        private readonly IDbContextFactory<PostgreSQLParametersDbContext> _contextFactory;

        public PostgreSQLParametersStore(IDbContextFactory<PostgreSQLParametersDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task Delete(string id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var p = context.PatientParameters.SingleOrDefault(x => x.Id == id);
            if (p != null)
            {
                context.PatientParameters.Remove(p);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAll(string patientId, DateTime timestamp)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var parameters = context.PatientParameters.AsQueryable().Where(x => x.PatientId == patientId && x.Timestamp == timestamp);
            foreach (var p in parameters)
                context.PatientParameters.Remove(p);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PatientParameter>> GetParameters(string patientId, DateTime start, DateTime end, List<string> names = null)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            IEnumerable<PatientParameter> parameters = context.PatientParameters.AsQueryable()
                .Where(x => x.PatientId == patientId && x.Timestamp <= end && x.Timestamp >= start)
                .ToList();

            if (parameters == null)
                return null;

            if (names != null)
            {
                var hash = new HashSet<string>(names);
                parameters = parameters.Where(x => names.Contains(x.Name));
            }

            parameters = parameters.GroupBy(x => x.Name)
                .Select(x => x.OrderByDescending(y => y.Timestamp).First());

            return await Task.FromResult(parameters);

        }

        public async Task Insert(PatientParameter p)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            if (p.Id == null)
                await context.PatientParameters.AddAsync(p);
            else
            {
                var dbParam = context.PatientParameters.SingleOrDefault(x => x.Id == p.Id);
                if (dbParam == null)
                    await context.PatientParameters.AddAsync(p);
                else
                {
                    dbParam.Name = p.Name;
                    dbParam.Value = p.Value;
                    dbParam.Timestamp = p.Timestamp;
                    dbParam.PatientId = p.PatientId;
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task Insert(IEnumerable<PatientParameter> parameters)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            foreach (var p in parameters)
            {
                if (p.Id == null)
                    await context.PatientParameters.AddAsync(p);
                else
                {
                    var dbParam = context.PatientParameters.SingleOrDefault(x => x.Id == p.Id 
                    || (x.PatientId == p.PatientId && x.Name == p.Name && x.Timestamp == p.Timestamp));
                    if (dbParam == null)
                        await context.PatientParameters.AddAsync(p);
                    else
                    {
                        dbParam.Name = p.Name;
                        dbParam.Value = p.Value;
                        dbParam.Timestamp = p.Timestamp;
                        dbParam.PatientId = p.PatientId;
                    }
                }


            }
            await context.SaveChangesAsync();
        }
    }
}
