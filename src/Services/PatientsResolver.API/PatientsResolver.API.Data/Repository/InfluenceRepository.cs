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
        public InfluenceRepository(IDbContextFactory<PatientsDataDbContext> dbContextFactory) : 
            base(dbContextFactory)
        {
            
        }


        public async Task<bool> AddPatientInluence(Influence influence, CancellationToken cancellationToken)
        {
#warning По хорошему это нужно вынести в команду. В ту, в которой сейчас вызывается этот метод.          
            IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using (var t = await dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (!IsCorrectInfluence(influence))
                            throw new Exception("Influence is not in valid format");

                        if (await IsInluenceExistAsync(dbContext, influence))
                            return false;

                        CheckParametersValues(influence);

                        Patient patient = await dbContext
                            .Patients
                            .FirstOrDefaultAsync(x => x.Id == influence.PatientId && x.MedicalOrganization == influence.MedicalOrganization);

#warning В данной реализации не добавляется пациент, если здесь не был найден. Просто пробрасывается ошибка.
                        if (patient == null)
                            throw new NullReferenceException($"Patient was not find with patientId = {influence.PatientId}");
                        else
                            await ProcessPatientAsync(patient, influence, cancellationToken);

                        await dbContext.Influences.AddAsync(influence, cancellationToken);
                        await dbContext.SaveChangesAsync();

                        if (influence.StartParameters != null) //после PatientDatas SaveChangesAsync для установки айдишников
                            await ProcessParametersAsync(dbContext, influence.Id, influence.StartParameters.Values, cancellationToken);
                        if (influence.DynamicParameters != null)
                            await ProcessParametersAsync(dbContext, influence.Id, influence.DynamicParameters.Values, cancellationToken);

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


        private void CheckParametersValues(Influence influence)
        {
#warning Выглядит костыльно
            foreach (PatientParameter p in influence.StartParameters.Values)
                if (p.NameTextDescription == null || p.NameTextDescription == string.Empty)
                    p.NameTextDescription = p.ParameterName.GetDisplayAttributeValue();
            foreach(PatientParameter p in influence.DynamicParameters.Values)
                if (p.NameTextDescription == null || p.NameTextDescription == string.Empty)
                    p.NameTextDescription = p.ParameterName.GetDisplayAttributeValue();
        }


        private bool IsCorrectInfluence(Influence inf) => inf != null
           && inf.MedicineName != null
           //&& inf.MedicineName != "" fix for data inserting
           //&& inf.InfluenceType != InfluenceTypes.None fix for data inserting
           && inf.PatientId > 0
           //&& inf.StartTimestamp != default(DateTime) fix for data inserting
           && inf.EndTimestamp != default(DateTime)
           && inf.StartParameters != null
           && inf.DynamicParameters != null
           && (inf.Patient != null? inf.Patient.Id == inf.PatientId : true);


        private async Task ProcessParametersAsync(PatientsDataDbContext dbContext, int influenceId, IEnumerable<PatientParameter> parameters, CancellationToken cancellationToken)
        {
            foreach (PatientParameter parameter in parameters)
                parameter.InfluenceId = influenceId;
            await dbContext.PatientsParameters.AddRangeAsync(parameters, cancellationToken);
            await dbContext.SaveChangesAsync();
        }


        private async Task ProcessPatientAsync(Patient patient, Influence influence, CancellationToken cancellationToken)
        {
            influence.PatientId = patient.Id;
            influence.Patient = patient;
        }


        private async Task<bool> IsInluenceExistAsync(PatientsDataDbContext dbContext, Influence influence)
        {
            List<Influence> data = await dbContext
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


        public async Task<List<Influence>> GetPatientInfluences(int patientId, string medicalOrganization,
            DateTime startTimestamp, DateTime endTimestamp, bool includeParams = true)
        {
            
            IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                var a = dbContext.Influences.ToArray();
                IQueryable<Influence> patientDatas = dbContext
                        .Influences
                        .Where(x => x.PatientId == patientId 
                                 && x.MedicalOrganization == medicalOrganization 
                                 && x.StartTimestamp >= startTimestamp 
                                 && x.EndTimestamp <= endTimestamp);

                if (patientDatas.Count() == 0)
                    return new List<Influence>();

                List<Influence> datas = await patientDatas
                    .Include(x => x.Patient)
                    .ToListAsync();

                if(includeParams)
                    InitParameters(dbContext, datas);

                return datas;
            });
        }


        public async Task<List<Influence>> GetInfluences(DateTime startTimestamp, DateTime endTimestamp, bool includeParams = true)
        {
            
            IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                IQueryable<Influence> influences = dbContext
                        .Influences
                        .Where(x => x.StartTimestamp >= startTimestamp && x.EndTimestamp <= endTimestamp);
                if (influences.Count() == 0)
                    return new List<Influence>();

                List<Influence> datas = await influences
                    .Include(x => x.Patient)
                    .ToListAsync();

                if (includeParams)
                    InitParameters(dbContext, datas);

                return datas;
            });
            
        }


        private void InitParameters(PatientsDataDbContext dbContext, List<Influence> datas)
        {
            datas.ForEach(x =>
            {
                try
                {
                    InitParametersFor(dbContext, x);
                }
                catch (Exception ex)
                {
#warning Ошибка контекста - две операции одновременно (примерно так звучит). Пока оставлен костыль на пропуск.
                    return;
                }
            });
        }


        private void InitParametersFor(PatientsDataDbContext dbContext, Influence influence)
        {
            IQueryable<PatientParameter> parameters = dbContext
                .PatientsParameters
                .Where(y => y.InfluenceId == influence.Id && y.MedicalOrganization == influence.MedicalOrganization);
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


        public async Task<Influence?> GetPatientInfluence(int inluenceId, string medicalOrganization)
        {
            
            IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                Influence? inf = 
                await dbContext.Influences.FirstOrDefaultAsync(x => x.Id == inluenceId && x.MedicalOrganization == medicalOrganization);
                if (inf != null)
                    InitParametersFor(dbContext, inf);
                return inf;

            });
            
        }

        public async Task<Influence> UpdateInfluence(Influence influence, CancellationToken cancellationToken)
        {
            
            IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
            if (influence.Id == default(int))
                throw new KeyNotFoundException("Id переданного воздействия не установлен");
            if (!IsCorrectInfluence(influence))
                throw new Exception("Обновление воздействия отменено, передано некорректное воздействие");
            return await strategy.ExecuteAsync(async () =>
            {
                using (var t = await dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Influence dbInfluence = await GetPatientInfluence(influence.Id, influence.MedicalOrganization);
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
                await ProcessParametersAsync(dbContext, dbInfluence.Id, startParamsToAdd.Concat(dynamicParamsToAdd), cancellationToken);

            foreach (PatientParameter p in startParamsToUpdate)
                CopyFieldsToDbParameter(dbContext, p, dbInfluence.StartParameters[p.ParameterName]);
            foreach (PatientParameter p in dynamicParamsToUpdate)
                CopyFieldsToDbParameter(dbContext, p, dbInfluence.DynamicParameters[p.ParameterName]);

            foreach (PatientParameter p in paramsToDelete)
                dbContext.Entry(p).State = EntityState.Deleted;

            dbContext.Entry(dbInfluence).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();
            
        }


        private void CopyFieldsToDbParameter(PatientsDataDbContext dbContext, PatientParameter from, PatientParameter dbParameter)
        {
            dbParameter.Value = from.Value;
            dbParameter.IsDynamic = from.IsDynamic;
            dbParameter.IsDynamic = from.IsDynamic;
            dbParameter.NameTextDescription = from.NameTextDescription;
            dbParameter.PositiveDynamicCoef = from.PositiveDynamicCoef;
            dbContext.Entry(dbParameter).State = EntityState.Modified;
        }

        public async Task<bool> DeleteInfluence(int influenceId, string medicalOrganization, CancellationToken cancellationToken)
        {
            
            IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
            Influence inf = await GetPatientInfluence(influenceId, medicalOrganization);
            if (inf == null)
                throw new KeyNotFoundException($"Не найдено воздействие с id = {influenceId}");
            return await strategy.ExecuteAsync(async () =>
            {
                using (var t = await dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        dbContext.Entry(inf).State = EntityState.Deleted;
                        foreach (PatientParameter p in inf.StartParameters.Values)
                            dbContext.Entry(p).State = EntityState.Detached;
                        foreach (PatientParameter p in inf.DynamicParameters.Values)
                            dbContext.Entry(p).State = EntityState.Detached;
                        await dbContext.SaveChangesAsync();
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
    }
}
