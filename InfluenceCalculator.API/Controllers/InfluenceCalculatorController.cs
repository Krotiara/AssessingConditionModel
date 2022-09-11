using InfluenceCalculator.API.Models;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfluenceCalculator.API.Controllers
{

    public class InfluenceCalculatorController: Controller
    {
        InfluenceModel influenceModel;

        public InfluenceCalculatorController()
        {
            influenceModel = new InfluenceModel();
        }


        [HttpGet("calculate/{influenceName}")]
        public ActionResult<IInfluenceResult> CalculateInfluence(string influenceName, [FromBody] IPatientData patientData)
        {
            try
            {
                IInfluenceResult influenceResult = influenceModel.CalculateInfluence(influenceName, patientData);
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
        public IActionResult SaveInfluenceResult([FromBody] IInfluenceResult influenceResult)
        {
            throw new NotImplementedException();
        }
    }
}
