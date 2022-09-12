using InfluenceCalculator.API.Controllers;
using InfluenceCalculator.API.Models;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            mock.Setup(r=>r.Influence).Returns(new Influence()
            {
                InfluenceType = InfluenceTypes.BiologicallyActiveAdditive,
                MedicineName = "medicine",
                StartTimestamp = DateTime.Now
            });

            var mockSet = new Mock<DbSet<IInfluenceResult>>();
            var mockContext = new Mock<InfluenceContext>();
            mockContext.Setup(m => m.InfluenceResults).Returns(mockSet.Object);


            InfluenceCalculatorController c = new InfluenceCalculatorController(mockContext.Object);
            var result = c.CalculateInfluence(mock.Object);

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