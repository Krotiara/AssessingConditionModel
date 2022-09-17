using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public class PatientDataRepository : Repository<PatientData>, IPatientDatarepository
    {

        public PatientDataRepository(PatientsDataDbContext patientsDataDbContext)
            :base(patientsDataDbContext)
        {

        }

        public async Task<Patient> GetPatientDataByIdAsync(long patientMedHistoryNumber, CancellationToken cancellationToken)
        {
            return await PatientsDataDbContext
                .Patients
                .FirstOrDefaultAsync(x => x.MedicalHistoryNumber == patientMedHistoryNumber);
        }

    }
}
