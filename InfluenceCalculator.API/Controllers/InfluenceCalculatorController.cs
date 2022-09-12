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


        [HttpGet("calculateInfluence")]
        public ActionResult<IInfluenceResult> CalculateInfluence([FromBody] IPatientData patientData)
        {
            try
            {
                IInfluenceResult influenceResult = influenceModel.CalculateInfluence(patientData);
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
    }
}
