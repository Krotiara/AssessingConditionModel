using Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using PatientDataHandler.API.Controllers;
using PatientDataHandler.API.Models;

namespace PatientDataHandler.UnitTests
{
    public class DataHandlerControllerTests
    {
        [Fact]
        public void ParseExcelDataTestMustCallDataProviderParseAndResolveParser()
        {
            Mock<IDataProvider> dataProviderMock = new Mock<IDataProvider>();
            dataProviderMock.Setup(x => x.ParseData(It.IsAny<string>())).Returns(GetTestData());

            Mock<Func<DataParserTypes, IDataProvider>> dataProviderResolverMock
               = new Mock<Func<DataParserTypes, IDataProvider>>();
            dataProviderResolverMock.Setup(x => x.Invoke(It.IsAny<DataParserTypes>())).Returns(dataProviderMock.Object);

            //var mockSet = new Mock<DbSet<PatientParameter>>();
            //var mockContext = new Mock<PatientsDataDbContext>();
            //mockContext.Setup(m => m.PatientsParameters).Returns(mockSet.Object);

            DataHandlerController controller = new DataHandlerController(dataProviderResolverMock.Object/*, mockContext.Object*/);

            string projectDirectory = Directory
                .GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            string pathToTestFiles = Path.Combine(projectDirectory, "TestFiles/ExcelDataTestFormat.xlsx");

            var result = controller.ParsePatientData(pathToTestFiles);

            dataProviderMock.Verify(mock => mock.ParseData(It.IsAny<string>()), Times.Once);
            dataProviderResolverMock.Verify(mock => mock.Invoke(It.IsAny<DataParserTypes>()), Times.Once);
        }


        private IList<IPatientData> GetTestData()
        {
            return new List<IPatientData>()
            {
                new PatientData()
                {
                    InfluenceId = 1,   
                    PatientId = 1,
                    Parameters = new List<IPatientParameter>()
                    {
                        new PatientParameter()
                        {Name = "testP1",PatientId = 1, Timestamp = DateTime.Now, Value = "15", DynamicValue = "10", PositiveDynamicCoef = 1 },
                        new PatientParameter()
                        {Name = "testP2",PatientId = 1, Timestamp = DateTime.Now, Value = "true", DynamicValue = "false", PositiveDynamicCoef = 1 }
                    }
                },
                new PatientData()
                {
                    InfluenceId = 2,    
                    PatientId = 2,
                    Parameters = new List<IPatientParameter>()
                    {
                        new PatientParameter()
                        {Name = "testP1",PatientId = 2, Timestamp = DateTime.Now, Value = "40", DynamicValue = "20", PositiveDynamicCoef = 1 },
                        new PatientParameter()
                        {Name = "testP2",PatientId = 2, Timestamp = DateTime.Now, Value = "30", DynamicValue = "50", PositiveDynamicCoef = -1 }
                    }
                }
            };
        }
    }
}