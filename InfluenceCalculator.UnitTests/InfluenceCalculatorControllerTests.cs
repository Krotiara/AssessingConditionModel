using InfluenceCalculator.API.Controllers;
using InfluenceCalculator.API.Models;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InfluenceCalculator.UnitTests
{
    public class InfluenceCalculatorControllerTests
    {
        [Fact]
        public void CalculateInfluenceMustReturnAActionResultWithInfluenceResult()
        {
            int userId = 1;
            Mock<IPatientData> mock = new Mock<IPatientData>();
            mock.Setup(r => r.Parameters).Returns(GetTestParameters(userId));
            mock.Setup(r => r.PatientId).Returns(userId);

            InfluenceCalculatorController c = new InfluenceCalculatorController();
            var result = c.CalculateInfluence("testInfluence", mock.Object);

            var actionResult = Assert.IsAssignableFrom<ActionResult<IInfluenceResult>>(result);
            var model = Assert.IsAssignableFrom<IInfluenceResult>((actionResult.Result as ObjectResult).Value);
        }


        private IEnumerable<PatientParameter> GetTestParameters(int userId)
        {
            
            DateTime dateTime = DateTime.Now;
            return new List<PatientParameter>()
            {
                new PatientParameter(userId, dateTime, "param1", true, 1, false),
                new PatientParameter(userId, dateTime, "param2", 40, -1, 20)
            };
        }
    }
}