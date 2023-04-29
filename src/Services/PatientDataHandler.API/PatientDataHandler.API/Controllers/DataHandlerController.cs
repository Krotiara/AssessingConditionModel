using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientDataHandler.API.Entities;
using PatientDataHandler.API.Models;
using PatientDataHandler.API.Service.Services;

namespace PatientDataHandler.API.Controllers
{
    public class DataHandlerController: ControllerBase
    {
        Func<DataParserTypes, IDataProvider> _dataParserResolver;


        public DataHandlerController(Func<DataParserTypes, IDataProvider> dataParserResolver)
        {
            this._dataParserResolver = dataParserResolver;
        }



        [HttpPost("parseInfluences/")]
        public ActionResult<IList<Influence>> ParseInfluencesData([FromBody] byte[] buffer)
        {
            try
            {
                // TODO try catch
                //TODO определение типа данных
                IDataProvider dataProvider = _dataParserResolver.Invoke(DataParserTypes.TestVahitova);
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
