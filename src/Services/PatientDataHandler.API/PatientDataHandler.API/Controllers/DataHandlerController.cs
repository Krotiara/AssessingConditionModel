using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientDataHandler.API.Entities;
using PatientDataHandler.API.Models;

namespace PatientDataHandler.API.Controllers
{
    public class DataHandlerController: Controller
    {
        Func<DataParserTypes, IDataProvider> dataParserResolver;


        public DataHandlerController(Func<DataParserTypes, IDataProvider> dataParserResolver)
        {
            this.dataParserResolver = dataParserResolver;
        }


        [HttpGet("parseData/{pathToFile}")]
        public ActionResult<IList<IPatientData>> ParsePatientData(string pathToFile)
        {
            // TODO try catch
            //TODO определение типа данных
            IDataProvider dataProvider = dataParserResolver.Invoke(DataParserTypes.TestVahitova);
            IList<IPatientData> patientDatas = dataProvider.ParseData(pathToFile);
            return Ok(patientDatas);
        }
    }
}
