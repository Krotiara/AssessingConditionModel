using InfluenceCalculator.API.Models;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfluenceCalculator.API.Controllers
{

    public class InfluenceCalculatorController: Controller
    {
        InfluenceModel influenceModel;
        InfluenceContext dbContext;

        public InfluenceCalculatorController(InfluenceContext dbContext)
        {
            this.dbContext = dbContext;
            influenceModel = new InfluenceModel();
        }


        [HttpGet("calculate/{influenceName}")]
        public ActionResult<IInfluenceResult> CalculateInfluence(int influenceId, [FromBody] IPatientData patientData)
        {
            try
            {
                IInfluenceResult influenceResult = influenceModel.CalculateInfluence(influenceId, patientData);
                return Ok(influenceResult);
            }
            catch(InfluenceCalculationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest($"Unexpected error: {ex.Message}.");
            }
        }


        [HttpGet("history/{patientId}")]
        public ActionResult<IEnumerable<IInfluenceResult>> GetInfluenceHistory(int patientId)
        {
            throw new NotImplementedException();
        }


        [HttpPost("save")]
        public async Task<IActionResult> SaveInfluenceResultAsync([FromBody] IInfluenceResult influenceResult)
        {
            try
            {
                //TODo разобраться, как быть в таких ситуациях, когда нужно сохранить экземпляр, а передается не экземпляр
                //if (influenceResult as InfluenceResult == null)
                //    return BadRequest();
                await dbContext.InfluenceResults.AddAsync(influenceResult);
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex); //TODo более осмысленных catch
            }
        }
    }
}
