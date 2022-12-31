using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public class PatientsRepository: Repository<Patient>
    {
        public PatientsRepository(IDbContextFactory<PatientsDataDbContext> dbContextFactory)
           : base(dbContextFactory)
        {

        }

        public async Task<Patient?> GetPatientBy(int medicalHistoryNumber)
        {
            return await dbContext
                .Patients.FirstOrDefaultAsync(x => x.MedicalHistoryNumber == medicalHistoryNumber);
        }
    }
}
