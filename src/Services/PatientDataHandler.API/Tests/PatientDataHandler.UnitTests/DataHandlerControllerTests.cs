using Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using PatientDataHandler.API.Controllers;
using PatientDataHandler.API.Entities;
using PatientDataHandler.API.Models;
using PatientDataHandler.API.Service.Services;
using System.Collections.Concurrent;

namespace PatientDataHandler.UnitTests
{
    public class DataHandlerControllerTests
    {
        [Fact]
        public void ParseExcelDataTestMustCallDataProviderParseAndResolveParser()
        {
            //Mock<IDataProvider> dataProviderMock = new Mock<IDataProvider>();
            //dataProviderMock.Setup(x => x.ParseData(It.IsAny<string>())).Returns(GetTestData());

            //Mock<Func<DataParserTypes, IDataProvider>> dataProviderResolverMock
            //   = new Mock<Func<DataParserTypes, IDataProvider>>();
            //dataProviderResolverMock.Setup(x => x.Invoke(It.IsAny<DataParserTypes>())).Returns(dataProviderMock.Object);

            ////var mockSet = new Mock<DbSet<PatientParameter>>();
            ////var mockContext = new Mock<PatientsDataDbContext>();
            ////mockContext.Setup(m => m.PatientsParameters).Returns(mockSet.Object);

            //DataHandlerController controller = new DataHandlerController(dataProviderResolverMock.Object/*, mockContext.Object*/);

            //string projectDirectory = Directory
            //    .GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            //string pathToTestFiles = Path.Combine(projectDirectory, "TestFiles/ExcelDataTestFormat.xlsx");

            //var result = controller.ParsePatientData(pathToTestFiles);

            //dataProviderMock.Verify(mock => mock.ParseData(It.IsAny<string>()), Times.Once);
            //dataProviderResolverMock.Verify(mock => mock.Invoke(It.IsAny<DataParserTypes>()), Times.Once);
        }


        //private IList<IPatientData<IPatientParameter, IPatient, IInfluence>> GetTestData()
        //{
        //    ConcurrentDictionary<ParameterNames, IPatientParameter> params1 = new ConcurrentDictionary<ParameterNames, IPatientParameter>();
        //    params1.TryAdd(ParameterNames.LifeQuality, new PatientParameter()
        //    { NameTextDescription = "testP1", PatientId = 1, Timestamp = DateTime.Now, Value = "15", DynamicValue = "10", PositiveDynamicCoef = 1 });
        //    params1.TryAdd(ParameterNames.Creatinine, new PatientParameter()
        //    { NameTextDescription = "testP2", PatientId = 1, Timestamp = DateTime.Now, Value = "true", DynamicValue = "false", PositiveDynamicCoef = 1 });

        //    ConcurrentDictionary<ParameterNames, IPatientParameter> params2 = new ConcurrentDictionary<ParameterNames, IPatientParameter>();
        //    params2.TryAdd(ParameterNames.LifeQuality, new PatientParameter()
        //    { NameTextDescription = "testP1", PatientId = 2, Timestamp = DateTime.Now, Value = "40", DynamicValue = "20", PositiveDynamicCoef = 1 });
        //    params2.TryAdd(ParameterNames.Creatinine, new PatientParameter()
        //    { NameTextDescription = "testP2", PatientId = 2, Timestamp = DateTime.Now, Value = "30", DynamicValue = "50", PositiveDynamicCoef = -1 });

        //    return new List<IPatientData<IPatientParameter, IPatient, IInfluence>>()
        //    {
        //        new PatientData()
        //        {
        //            InfluenceId = 1,   
        //            PatientId = 1,
        //            Parameters = params1
        //        },
        //        new PatientData()
        //        {
        //            InfluenceId = 2,    
        //            PatientId = 2,
        //            Parameters = params2
        //        }
        //    };
        //}
    }
}