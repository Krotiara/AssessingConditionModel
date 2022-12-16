using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
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


        public async Task<bool> AddPatientInluence(Influence influence, CancellationToken cancellationToken)
        {
#warning По хорошему это нужно вынести в команду. В ту, в которой сейчас вызывается этот метод.
            IExecutionStrategy strategy = PatientsDataDbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using (var t = await PatientsDataDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (!IsCorrectInfluence(influence))
                            throw new Exception("Influence is not in valid format");

                        if (await IsInluenceExistAsync(influence))
                            return false;

                        Patient patient = await PatientsDataDbContext
                            .Patients
                            .FirstOrDefaultAsync(x => x.MedicalHistoryNumber == influence.PatientId);

#warning В данной реализации не добавляется пациент, если здесь не был найден. Просто пробрасывается ошибка.
                        if (patient == null)
                            throw new NullReferenceException($"Patient was not find with patientId = {influence.PatientId}");
                        else
                            await ProcessPatientAsync(patient, influence, cancellationToken);

                        await PatientsDataDbContext.Influences.AddAsync(influence, cancellationToken);
                        await PatientsDataDbContext.SaveChangesAsync();

                        if (influence.StartParameters != null) //после PatientDatas SaveChangesAsync для установки айдишников
                            await ProcessParametersAsync(influence.Id, influence.StartParameters.Values, cancellationToken);
                        if (influence.DynamicParameters != null)
                            await ProcessParametersAsync(influence.Id, influence.DynamicParameters.Values, cancellationToken);

                        await t.CommitAsync(cancellationToken);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        await t.RollbackAsync(cancellationToken);
                        throw; //TODO
                    }
                }
            });
        }


        private bool IsCorrectInfluence(Influence inf) => inf != null
           && inf.MedicineName != null
           && inf.MedicineName != ""
           && inf.InfluenceType != InfluenceTypes.None
           && inf.PatientId > 0
           && inf.StartTimestamp != default(DateTime)
           && inf.EndTimestamp != default(DateTime)
           && inf.StartParameters != null
           && inf.DynamicParameters != null
           && (inf.Patient != null? inf.Patient.MedicalHistoryNumber == inf.PatientId : true);


        private async Task ProcessParametersAsync(int influenceId, IEnumerable<PatientParameter> parameters, CancellationToken cancellationToken)
        {
            foreach (PatientParameter parameter in parameters)
                parameter.InfluenceId = influenceId;
            await PatientsDataDbContext.PatientsParameters.AddRangeAsync(parameters, cancellationToken);
            await PatientsDataDbContext.SaveChangesAsync();
        }


        private async Task ProcessPatientAsync(Patient patient, Influence influence, CancellationToken cancellationToken)
        {
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
            datas.ForEach(x => { InitParametersFor(x);});
        }


        private void InitParametersFor(Influence influence)
        {
            IQueryable<PatientParameter> parameters = PatientsDataDbContext
                .PatientsParameters
                .Where(y => y.InfluenceId == influence.Id);
            foreach (PatientParameter p in parameters)
                try
                {
                    p.ParameterName = p.NameTextDescription.GetParameterByDescription(); //TODO Может есть выход лучше?
                    if (p.IsDynamic)
                        influence.DynamicParameters[p.ParameterName] = p;
                    else
                        influence.StartParameters[p.ParameterName] = p;
                }
                catch (Exception ex)
                {
                    //TODO log
                    continue;
                }
        }


        public async Task<Influence?> GetPatientInfluence(int inluenceId)
        {
            IExecutionStrategy strategy = PatientsDataDbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                Influence? inf = await PatientsDataDbContext.Influences.FirstOrDefaultAsync(x=>x.Id == inluenceId);
                if (inf != null)
                    InitParametersFor(inf);
                return inf;
                    
            });
        }

        public async Task<Influence> UpdateInfluence(Influence influence, CancellationToken cancellationToken)
        {
            IExecutionStrategy strategy = PatientsDataDbContext.Database.CreateExecutionStrategy();
            if (influence.Id == default(int))
                throw new KeyNotFoundException("Id переданного воздействия не установлен");
            if (!IsCorrectInfluence(influence))
                throw new Exception("Обновление воздействия отменено, передано некорректное воздействие");
            return await strategy.ExecuteAsync(async () =>
            {
                using (var t = await PatientsDataDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Influence dbInfluence = await GetPatientInfluence(influence.Id);
                        await CopyFieldsToDbInfuence(influence, dbInfluence, cancellationToken);
                        await t.CommitAsync(cancellationToken);
                        return dbInfluence;
                    }
                    catch (Exception ex)
                    {
                        await t.RollbackAsync(cancellationToken);
                        throw; //TODO
                    }
                }
            });
        }


        private async Task CopyFieldsToDbInfuence(Influence from, Influence dbInfluence, CancellationToken cancellationToken)
        {
            //TODO заменить на .Copy Без пересоздания элемента
            dbInfluence.InfluenceType = from.InfluenceType;
            dbInfluence.PatientId = from.PatientId;
            dbInfluence.Patient = from.Patient;
            dbInfluence.StartTimestamp = from.StartTimestamp;
            dbInfluence.EndTimestamp = from.EndTimestamp;
            dbInfluence.MedicineName = from.MedicineName;
            

            //TODO рефакторинг
            IEnumerable<PatientParameter> startParamsToAdd = 
                from.StartParameters.Where(x => !dbInfluence.StartParameters.ContainsKey(x.Key)).Select(x=>x.Value);
            IEnumerable<PatientParameter> startParamsToUpdate = 
                from.StartParameters.Where(x=> dbInfluence.StartParameters.ContainsKey(x.Key)).Select(x => x.Value);
            IEnumerable<PatientParameter> startParamsToDelete = 
                dbInfluence.StartParameters.Where(x => !from.StartParameters.ContainsKey(x.Key)).Select(x => x.Value);
            IEnumerable<PatientParameter> dynamicParamsToAdd = 
                from.DynamicParameters.Where(x => !dbInfluence.DynamicParameters.ContainsKey(x.Key)).Select(x => x.Value);
            IEnumerable<PatientParameter> dynamicParamsToUpdate = 
                from.DynamicParameters.Where(x => dbInfluence.DynamicParameters.ContainsKey(x.Key)).Select(x => x.Value);
            IEnumerable<PatientParameter> paramsToDelete = (dbInfluence
                .StartParameters.Where(x => !from.StartParameters.ContainsKey(x.Key)).Select(x => x.Value))
                .Concat(dbInfluence.DynamicParameters.Where(x => !from.DynamicParameters.ContainsKey(x.Key)).Select(x => x.Value));

            IEnumerable<PatientParameter> paramsToAdd = startParamsToAdd.Concat(dynamicParamsToAdd);
            if (paramsToAdd.Any())
                await ProcessParametersAsync(dbInfluence.Id, startParamsToAdd.Concat(dynamicParamsToAdd), cancellationToken);

            foreach(PatientParameter p in startParamsToUpdate)
                CopyFieldsToDbParameter(p, dbInfluence.StartParameters[p.ParameterName]);
            foreach (PatientParameter p in dynamicParamsToUpdate)
                CopyFieldsToDbParameter(p, dbInfluence.DynamicParameters[p.ParameterName]);

            foreach (PatientParameter p in paramsToDelete)
                PatientsDataDbContext.Entry(p).State = EntityState.Deleted;

            PatientsDataDbContext.Entry(dbInfluence).State = EntityState.Modified;

            await PatientsDataDbContext.SaveChangesAsync();
        }


        private void CopyFieldsToDbParameter(PatientParameter from, PatientParameter dbParameter)
        {
            dbParameter.Value = from.Value;
            dbParameter.IsDynamic = from.IsDynamic;
            dbParameter.IsDynamic = from.IsDynamic;
            dbParameter.NameTextDescription = from.NameTextDescription;
            dbParameter.PositiveDynamicCoef = from.PositiveDynamicCoef;
            PatientsDataDbContext.Entry(dbParameter).State = EntityState.Modified;
        }
    }
}
