using Interfaces;
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


        public async Task<List<PatientData>> GetPatientData(int patientId, 
            DateTime startTimestamp, DateTime endTimestamp)
        {
            IExecutionStrategy strategy = PatientsDataDbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                IQueryable<PatientData> patientDatas = PatientsDataDbContext
                        .PatientDatas
                        .Where(x => x.PatientId == patientId)
                        .Where(x => x.Timestamp >= startTimestamp && x.Timestamp <= endTimestamp);

                if (patientDatas.Count() == 0)
                    return new List<PatientData>();

                List<PatientData> datas = await patientDatas
                    .Include(x => x.Patient)
                    //.Include(x => x.Parameters)
                    .ToListAsync();

                datas.ForEach(x =>
                {
                    IQueryable<PatientParameter> parameters = PatientsDataDbContext
                    .PatientsParameters
                    .Where(y => y.PatientDataId == x.Id);
                    foreach (PatientParameter p in parameters)
                        try
                        {
                            p.ParameterName = p.NameTextDescription.GetParameterByDescription(); //TODO Может есть выход лучше?
                            x.Parameters[p.ParameterName] = p;
                        }
                        catch (Exception ex)
                        {
                            //TODO log
                            continue;
                        }
                });
                return datas;
            });
           
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
                        if (await IsPatientDataExistAsync(patientData))
                            return;

                        Patient patient = await PatientsDataDbContext
                            .Patients
                            .FirstOrDefaultAsync(x => x.MedicalHistoryNumber == patientData.PatientId);

                        await ProcessPatientAsync(patient, patientData, cancellationToken);
                        await ProcessInfluenceAsync(patientData, cancellationToken);

                        await PatientsDataDbContext.PatientDatas.AddAsync(patientData, cancellationToken);
                        await PatientsDataDbContext.SaveChangesAsync();

                        if (patientData.Parameters != null) //после PatientDatas SaveChangesAsync для установки айдишников
                            await ProcessParametersAsync(patientData, patientData.Parameters.Values, cancellationToken);

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


        private async Task<bool> IsPatientDataExistAsync(PatientData patientData)
        {
            List<PatientData> data = await PatientsDataDbContext
                .PatientDatas.Where(x => x.PatientId == patientData.PatientId
                && x.Timestamp == patientData.Timestamp)
                .Include(x => x.Patient)
                .Include(x=>x.Influence)
                //.Include(x => x.Parameters)
                .Where(x => x.Influence != null)
                .ToListAsync();

            if (!data.Any())
                return false;
            foreach (PatientData pD in data)
                if (new InfluenceComparer().Equals(pD.Influence, patientData.Influence))
                    return true;
            return false;
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


        private async Task ProcessParametersAsync(PatientData patientData, IEnumerable<PatientParameter> parameters, CancellationToken cancellationToken)
        {
            foreach (PatientParameter parameter in parameters)
                parameter.PatientDataId = patientData.Id;
            await PatientsDataDbContext.PatientsParameters.AddRangeAsync(patientData.Parameters.Values, cancellationToken);
            await PatientsDataDbContext.SaveChangesAsync();
        }
    }
}
