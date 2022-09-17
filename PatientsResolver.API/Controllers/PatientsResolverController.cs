﻿using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Data;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Command;

namespace PatientsResolver.API.Controllers
{
    public class PatientsResolverController: Controller
    {
        private readonly PatientsDataDbContext patientsDataDbContext;
        private readonly IMediator mediator;

        public PatientsResolverController(PatientsDataDbContext patientsDataDbContext, IMediator mediator)
        {
            this.patientsDataDbContext = patientsDataDbContext;
            this.mediator = mediator;
        }


        [HttpGet("getPatientData/{patientId}")]
        public async Task<ActionResult<IList<IPatientData>>> GetPatientData(int patientId)
        {
            IQueryable<PatientData> patientDatas = patientsDataDbContext
                .PatientDatas
                .Where(x => x.PatientId == patientId);

            if (patientDatas.Count() == 0)
                return BadRequest("No patient data is found");

#warning Выскакивала ошибка The expression 'x.Parameters' is invalid inside an 'Include' operation
            List<PatientData> datas = await patientDatas
                .Include(x=>x.Patient)
                .Include(x => x.Parameters)
                .ToListAsync();
            return Ok(datas);
        }


        [HttpGet("getPatient/{patientId}")]
        public async Task<ActionResult<IPatient>> GetPatient(long patientMedicalHistoryNumber)
        {
            Patient? patient = await patientsDataDbContext
                .Patients
                .FirstOrDefaultAsync(x => x.MedicalHistoryNumber == patientMedicalHistoryNumber);

            if (patient == null)
                return BadRequest($"Patient with medical history number = {patientMedicalHistoryNumber} was not found.");

            return Ok(patient);
        }



        [HttpPost("saveData")]
        public async Task<ActionResult> SavePatientDataAsync([FromBody] IList<IPatientData> patientDatas)
        {
            try
            {
                using (var transaction = patientsDataDbContext.Database.BeginTransaction())
                {
                    //Рассмотреть необходимость сужения try catch до поэлементного отлова.
                    try
                    {
                        foreach (PatientData data in patientDatas)
                        {
                            //Не уверен, что upcast хороший выбор.
                            await patientsDataDbContext.PatientsParameters.AddRangeAsync(data.Parameters.Cast<PatientParameter>());
                            await patientsDataDbContext.PatientDatas.AddAsync(data);
                            await patientsDataDbContext.SaveChangesAsync();
                        }

                        await patientsDataDbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex) //TODO осмысленный try catch
                    {
                        //TODO add log
                        transaction.Rollback();
                        return BadRequest(ex.Message);
                    }
                }

                foreach (IPatientData patientData in patientDatas)
                    await mediator.Send(new UpdatePatientDataCommand() { PatientData = patientData });

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}