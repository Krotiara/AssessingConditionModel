using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientDataHandler.API.Entities;
using PatientDataHandler.API.Models;
using PatientDataHandler.API.Service.Services;

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
        public ActionResult<IList<Influence>> ParsePatientData(string pathToFile)
        {
            try
            {
                // TODO try catch
                //TODO определение типа данных
                IDataProvider dataProvider = dataParserResolver.Invoke(DataParserTypes.TestVahitova);
                var patientDatas = dataProvider.ParseData(pathToFile);
                return Ok(patientDatas);
            }
            catch (ParseInfluenceDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unexpected error:{ex.Message}");
            }
        }


        [HttpPost("parseInfluences/")]
        public ActionResult<IList<Influence>> ParseInfluencesData([FromBody] byte[] buffer)
        {
            try
            {
                // TODO try catch
                //TODO определение типа данных
                IDataProvider dataProvider = dataParserResolver.Invoke(DataParserTypes.TestVahitova);
                IList<Influence> influences = dataProvider.ParseData(buffer);
                return Ok(influences);
            }
            catch(ParseInfluenceDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest($"Unexpected error:{ex.Message}");
            }
        }
    }
}
