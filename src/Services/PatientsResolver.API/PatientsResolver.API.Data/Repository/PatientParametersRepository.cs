using Interfaces;
using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public class PatientParametersRepository: Repository<PatientParameter>
    {
        public PatientParametersRepository(IDbContextFactory<PatientsDataDbContext> dbContextFactory)
          : base(dbContextFactory)
        {

        }


        public async Task<List<PatientParameter>> GetLatestParameters(int patientId, string patientAffiliation, DateTime startTimestamp, DateTime endTimestamp)
        {
            
            List<PatientParameter> parameters =
                dbContext.PatientsParameters
                .Where(x => x.PatientId == patientId 
                         && x.PatientAffiliation == patientAffiliation 
                         && x.Timestamp >= startTimestamp 
                         && x.Timestamp <= endTimestamp)
                .ToList();

            var groupedParams = parameters.GroupBy(x => x.Name);
            List<PatientParameter> result = new List<PatientParameter>();
            foreach (IGrouping<string, PatientParameter> group in groupedParams)
                result.Add(group.OrderBy(x => x.Timestamp).Last());
            return result;
            
        }
    }
}
