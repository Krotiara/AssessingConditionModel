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

            var mockSet = new Mock<DbSet<IInfluenceResult>>();
            var mockContext = new Mock<InfluenceContext>();
            mockContext.Setup(m => m.InfluenceResults).Returns(mockSet.Object);


            InfluenceCalculatorController c = new InfluenceCalculatorController(mockContext.Object);
            var result = c.CalculateInfluence(1, mock.Object);

            var actionResult = Assert.IsAssignableFrom<ActionResult<IInfluenceResult>>(result);
            var model = Assert.IsAssignableFrom<IInfluenceResult>((actionResult.Result as ObjectResult).Value);
        }


        [Fact]
        public async Task SaveInfluenceTestAsync()
        {
            //List<IInfluenceResult> data = new List<IInfluenceResult>();
            //IQueryable<IInfluenceResult> queryable = data.AsQueryable();
            //mockSet.As<IQueryable<IInfluenceResult>>().Setup(m => m.Provider).Returns(queryable.Provider);
            //mockSet.As<IQueryable<IInfluenceResult>>().Setup(m => m.Expression).Returns(queryable.Expression);
            //mockSet.As<IQueryable<IInfluenceResult>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            //mockSet.As<IQueryable<IInfluenceResult>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            //mockSet.Setup(d => d.Add(It.IsAny<IInfluenceResult>())).Callback<IInfluenceResult>((s) => data.Add(s));

            var mockSet = new Mock<DbSet<IInfluenceResult>>();
            var mockContext = new Mock<InfluenceContext>();
            mockContext.Setup(m => m.InfluenceResults).Returns(mockSet.Object);

            InfluenceCalculatorController c = new InfluenceCalculatorController(mockContext.Object);

            InfluenceResult testResult = new InfluenceResult() { InfluenceId = 1, InfluenceEffectiveness = 1 };

            IActionResult res = await c.SaveInfluenceResultAsync(testResult);

            mockSet.Verify(x=>x.Add(It.IsAny<IInfluenceResult>()), Times.Once());
            mockContext.Verify(x=>x.SaveChanges(), Times.Once());
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