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
            IDataProvider dataProvider = dataParserResolver.Invoke(DataParserTypes.TestVahitova);
            IList<IPatientData> patientDatas = dataProvider.ParseData(pathToFile);
            return Ok(patientDatas);
        }
    }
}
