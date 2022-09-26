using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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


        public async Task AddPatientData(PatientData patientData, CancellationToken cancellationToken)
        {
            IExecutionStrategy strategy = PatientsDataDbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var t = await PatientsDataDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Patient patient = await PatientsDataDbContext
                            .Patients
                            .FirstOrDefaultAsync(x => x.MedicalHistoryNumber == patientData.Id);

                        await ProcessPatientAsync(patient, patientData, cancellationToken);
                        await ProcessInfluenceAsync(patientData, cancellationToken);

                        if (patientData.Parameters != null)
                        {
                            await PatientsDataDbContext.PatientsParameters.AddRangeAsync(patientData.Parameters, cancellationToken);
                            await PatientsDataDbContext.SaveChangesAsync();
                        }

                        await PatientsDataDbContext.PatientDatas.AddAsync(patientData, cancellationToken);
                        await PatientsDataDbContext.SaveChangesAsync();

                        await t.CommitAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        await t.RollbackAsync(cancellationToken);
                        throw;
                    }
                }
            });           
        }


        private async Task ProcessPatientAsync(Patient patient, PatientData patientData, CancellationToken cancellationToken)
        {
            if (patient == null)
            {
                patient = patientData.Patient != null ?
                    patientData.Patient :
                    new Patient()
                    {
                        Name = "",
                        MedicalHistoryNumber = patientData.PatientId,
                        Birthday = DateTime.MinValue
                    };

                await PatientsDataDbContext.Patients.AddAsync(patient, cancellationToken);
                await PatientsDataDbContext.SaveChangesAsync();          
            }
            patientData.PatientId = patient.MedicalHistoryNumber;
            patientData.Patient = patient;
            if(patientData.Influence != null)
                patientData.Influence.PatientId = patient.MedicalHistoryNumber;
        }


        private async Task ProcessInfluenceAsync(PatientData patientData, CancellationToken cancellationToken)
        {
            if (patientData.Influence != null)
            { 
                //TODO разобраться, как засунуть в EF свой EqualityComparer
                Influence influence = await PatientsDataDbContext
                .Influences.FirstOrDefaultAsync(x =>x.PatientId == patientData.Influence.PatientId 
                && x.MedicineName == patientData.Influence.MedicineName
                && x.StartTimestamp == patientData.Influence.StartTimestamp
                && x.EndTimestamp == patientData.Influence.EndTimestamp);

                if (influence != null)
                {
                    patientData.Influence = influence;
                    patientData.InfluenceId = influence.Id;
                }

                else //Добавить воздействие, если такого еще нет
                {
                    await PatientsDataDbContext.Influences.AddAsync(patientData.Influence, cancellationToken);
                    await PatientsDataDbContext.SaveChangesAsync();
                    patientData.InfluenceId = patientData.Influence.Id; //TODO мб избавиться от Id полей, раз есть сами сущности.
                }
            }
        }
    }
}
