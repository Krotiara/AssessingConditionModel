using Interfaces;
using Microsoft.AspNetCore.Mvc;
using PatientDataHandler.API.Models;

namespace PatientDataHandler.API.Controllers
{
    public class DataHandlerController: Controller
    {
        Func<DataParserTypes, IDataProvider> dataParserResolver;
        PatientsDataDbContext patientsDataDbContext;


        public DataHandlerController(Func<DataParserTypes, IDataProvider> dataParserResolver, PatientsDataDbContext patientsDataDbContext)
        {
            this.dataParserResolver = dataParserResolver;
            this.patientsDataDbContext = patientsDataDbContext;
        }


        [HttpGet("parseData/{pathToFile}")]
        public ActionResult<IList<IPatientData>> ParsePatientData(string pathToFile)
        {
            //TODO определение типа данных
            IDataProvider dataProvider = dataParserResolver.Invoke(DataParserTypes.TestVahitova);
            IList<IPatientData> patientDatas = dataProvider.ParseData(pathToFile);
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
                catch(Exception ex) //TODO осмысленный try catch
                {
                    //TODO add log
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
