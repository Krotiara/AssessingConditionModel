using InfluenceCalculator.API.Controllers;
using InfluenceCalculator.API.Models;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Collections.Concurrent;

namespace InfluenceCalculator.UnitTests
{
    public class InfluenceCalculatorControllerTests
    {
        [Fact]
        public void CalculateInfluenceMustReturnAActionResultWithInfluenceResult()
        {
            //int userId = 1;
            //Mock<IPatientData<IPatientParameter, IPatient, IInfluence>> mock = 
            //    new Mock<IPatientData<IPatientParameter, IPatient, IInfluence>>();
            //mock.Setup(r => r.Parameters).Returns(GetTestParameters(userId));
            //mock.Setup(r => r.PatientId).Returns(userId);
            //mock.Setup(r=>r.InfluenceId).Returns(1);

            //var mockSet = new Mock<DbSet<InfluenceResult>>();
            //InfluenceCalculatorController c = new InfluenceCalculatorController();
            //var result = c.CalculateInfluence(mock.Object);

            //var actionResult = Assert.IsAssignableFrom<ActionResult<IInfluenceResult>>(result);
            //var model = Assert.IsAssignableFrom<IInfluenceResult>((actionResult.Result as ObjectResult).Value);
        }

        private ConcurrentDictionary<ParameterNames, IPatientParameter> GetTestParameters(int userId)
        {
            
            DateTime dateTime = DateTime.Now;
            ConcurrentDictionary < ParameterNames, IPatientParameter > dict = new ConcurrentDictionary<ParameterNames, IPatientParameter> ();
            dict.TryAdd(ParameterNames.Urea, new PatientParameter(userId, dateTime, "param1", "true", 1, "false"));
            dict.TryAdd(ParameterNames.Creatinine, new PatientParameter(userId, dateTime, "param2", "40", -1, "20"));
            return dict;
        }

    }
}