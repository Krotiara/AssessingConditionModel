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
        public PatientsRepository(PatientsDataDbContext patientsDataDbContext)
           : base(patientsDataDbContext)
        {

        }

        public async Task<Patient?> GetPatientBy(int medicalHistoryNumber)
        {
            return await PatientsDataDbContext
                .Patients.FirstOrDefaultAsync(x => x.MedicalHistoryNumber == medicalHistoryNumber);

        }
    }
}
