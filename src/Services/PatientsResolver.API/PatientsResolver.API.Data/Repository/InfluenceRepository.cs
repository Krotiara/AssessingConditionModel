using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public class InfluenceRepository : Repository<Influence>, IInfluenceRepository
    {
        public InfluenceRepository(PatientsDataDbContext patientsDataDbContext): 
            base(patientsDataDbContext)
        {
            
        }


        public async Task AddPatientInluence(Influence influence, CancellationToken cancellationToken)
        {
            IExecutionStrategy strategy = PatientsDataDbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var t = await PatientsDataDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (await IsInluenceExistAsync(influence))
                            return;

                        Patient patient = await PatientsDataDbContext
                            .Patients
                            .FirstOrDefaultAsync(x => x.MedicalHistoryNumber == influence.PatientId);

                        await ProcessPatientAsync(patient, influence, cancellationToken);

                        await PatientsDataDbContext.Influences.AddAsync(influence, cancellationToken);
                        await PatientsDataDbContext.SaveChangesAsync();

                        if (influence.StartParameters != null) //после PatientDatas SaveChangesAsync для установки айдишников
                            await ProcessParametersAsync(influence.Id, influence.StartParameters.Values, cancellationToken);
                        if(influence.DynamicParameters != null)
                            await ProcessParametersAsync(influence.Id, influence.DynamicParameters.Values, cancellationToken);

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


        private async Task ProcessParametersAsync(int influenceId, IEnumerable<PatientParameter> parameters, CancellationToken cancellationToken)
        {
            foreach (PatientParameter parameter in parameters)
                parameter.InfluenceId = influenceId;
            await PatientsDataDbContext.PatientsParameters.AddRangeAsync(parameters, cancellationToken);
            await PatientsDataDbContext.SaveChangesAsync();
        }


        private async Task ProcessPatientAsync(Patient patient, Influence influence, CancellationToken cancellationToken)
        {
            if (patient == null)
            {
                patient = influence.Patient != null ?
                    influence.Patient :
                    new Patient()
                    {
                        Name = "",
                        MedicalHistoryNumber = influence.PatientId,
                        Birthday = DateTime.MinValue
                    };

                await PatientsDataDbContext.Patients.AddAsync(patient, cancellationToken);
                await PatientsDataDbContext.SaveChangesAsync();
            }
            influence.PatientId = patient.MedicalHistoryNumber;
            influence.Patient = patient;
        }


        private async Task<bool> IsInluenceExistAsync(Influence influence)
        {
            List<Influence> data = await PatientsDataDbContext
                .Influences.Where(x => x.PatientId == influence.PatientId && 
                x.StartTimestamp == influence.StartTimestamp &&
                x.EndTimestamp == influence.EndTimestamp)
                .Include(x => x.Patient)
                .ToListAsync();

            if (!data.Any())
                return false;
            foreach (Influence pD in data)
                if (new InfluenceComparer().Equals(pD, influence))
                    return true;
            return false;
        }

        public async Task<List<Influence>> GetPatientInfluences(int patientId, DateTime startTimestamp, DateTime endTimestamp)
        {
            IExecutionStrategy strategy = PatientsDataDbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                IQueryable<Influence> patientDatas = PatientsDataDbContext
                        .Influences
                        .Where(x => x.PatientId == patientId)
                        .Where(x => x.StartTimestamp >= startTimestamp && x.EndTimestamp <= endTimestamp);

                if (patientDatas.Count() == 0)
                    return new List<Influence>();

                List<Influence> datas = await patientDatas
                    .Include(x => x.Patient)
                    .ToListAsync();

                InitParameters(datas);

                return datas;
            });

        }

        public async Task<List<Influence>> GetInfluences(DateTime startTimestamp, DateTime endTimestamp)
        {
            IExecutionStrategy strategy = PatientsDataDbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                IQueryable<Influence> influences = PatientsDataDbContext
                        .Influences
                        .Where(x => x.StartTimestamp >= startTimestamp && x.EndTimestamp <= endTimestamp);
                if (influences.Count() == 0)
                    return new List<Influence>();

                List<Influence> datas = await influences
                   .Include(x => x.Patient)
                   .ToListAsync();

                InitParameters(datas);

                return datas;
            });
        }


        private void InitParameters(List<Influence> datas)
        {
            datas.ForEach(x =>
            {
                IQueryable<PatientParameter> parameters = PatientsDataDbContext
                .PatientsParameters
                .Where(y => y.InfluenceId == x.Id);
                foreach (PatientParameter p in parameters)
                    try
                    {
                        p.ParameterName = p.NameTextDescription.GetParameterByDescription(); //TODO Может есть выход лучше?
                        if (p.IsDynamic)
                            x.DynamicParameters[p.ParameterName] = p;
                        else
                            x.StartParameters[p.ParameterName] = p;
                    }
                    catch (Exception ex)
                    {
                        //TODO log
                        continue;
                    }
            });
        }
    }
}
