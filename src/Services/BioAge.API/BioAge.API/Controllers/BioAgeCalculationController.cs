using BioAge.API.Entites;
using BioAge.API.Service.Services;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BioAge.API.Controllers
{
    public class BioAgeCalculationController: Controller
    {
        private readonly IBioAgeCalculationService bioAgeCalculationService;
        private readonly IMediator mediator;

        public BioAgeCalculationController(IBioAgeCalculationService bioAgeCalculationService, IMediator mediator)
        {
            this.bioAgeCalculationService = bioAgeCalculationService;
            this.mediator = mediator;
        }


#warning Может переделать под другой формат входных параметров?
        [HttpGet("bioAge/")]
        public async Task<ActionResult<double>> GetBioAge([FromBody]BioAgeCalculationParameters calculationParams)
        {
            try
            {
                return Ok(await bioAgeCalculationService.CalculateBioAge(calculationParams));
            }
            catch(BioAgeCalculationException ex)
            {
                return BadRequest($"Bio age calculation error:{ex.Message}");
            }
            catch(Exception ex)
            {
                return BadRequest($"Unexpected error:{ex.Message}");
            }
        }
    }
}
