using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Models;

namespace PatientsResolver.API.Controllers
{
    public class PatientsResolverController: Controller
    {
        private PatientsDataDbContext patientsDataDbContext;

        public PatientsResolverController(PatientsDataDbContext patientsDataDbContext)
        {
            this.patientsDataDbContext = patientsDataDbContext;
        }


        [HttpGet("getData/{patientId}")]
        public async Task<ActionResult<IList<IPatientData>>> GetPatientData(int patientId)
        {
            IQueryable<PatientData> patientDatas = patientsDataDbContext
                .PatientDatas
                .Where(x => x.PatientId == patientId);

            if (patientDatas.Count() == 0)
                return BadRequest("No patient data is found");

#warning Выскакивала ошибка The expression 'x.Parameters' is invalid inside an 'Include' operation
            patientDatas = patientDatas.Include(x=>x.Patient).Include(x => x.Parameters);

            return Ok(patientDatas);
        }


        [HttpPost("saveData")]
        public async Task<ActionResult> SavePatientDataAsync([FromBody] IList<IPatientData> patientDatas)
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
                    return Ok();
                }
                catch (Exception ex) //TODO осмысленный try catch
                {
                    //TODO add log
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
