using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public class PatientDataRepository : Repository<PatientData>, IPatientDataRepository
    {

        public PatientDataRepository(PatientsDataDbContext patientsDataDbContext)
            :base(patientsDataDbContext)
        {

        }


        public async Task<Patient> GetPatientDataByIdAsync(int patientMedHistoryNumber, CancellationToken cancellationToken)
        {
            return await PatientsDataDbContext
                .Patients
                .FirstOrDefaultAsync(x => x.MedicalHistoryNumber == patientMedHistoryNumber);
        }


        public async Task<List<PatientData>> GetPatientData(int patientId)
        {
            IQueryable<PatientData> patientDatas = PatientsDataDbContext
                        .PatientDatas
                        .Where(x => x.PatientId == patientId);

            if (patientDatas.Count() == 0)
                return new List<PatientData>();

#warning Выскакивала ошибка The expression 'x.Parameters' is invalid inside an 'Include' operation
            List<PatientData> datas = await patientDatas
                .Include(x => x.Patient)
                .Include(x => x.Parameters)
                .ToListAsync();
            return datas;
        }

    }
}
