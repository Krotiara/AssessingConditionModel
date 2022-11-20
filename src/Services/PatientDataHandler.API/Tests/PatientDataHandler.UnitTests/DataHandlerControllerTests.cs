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
            Mock<IDataProvider> dataProviderMock = new Mock<IDataProvider>();
            dataProviderMock.Setup(x => x.ParseData(It.IsAny<byte[]>())).Returns(new List<Influence>());

            Mock<Func<DataParserTypes, IDataProvider>> dataProviderResolverMock
               = new Mock<Func<DataParserTypes, IDataProvider>>();
            dataProviderResolverMock.Setup(x => x.Invoke(It.IsAny<DataParserTypes>())).Returns(dataProviderMock.Object);

            DataHandlerController controller = new DataHandlerController(dataProviderResolverMock.Object);   
            var result = controller.ParseInfluencesData(GetCorrectTestDataBytes());

            dataProviderMock.Verify(mock => mock.ParseData(It.IsAny<byte[]>()), Times.Once);
            dataProviderResolverMock.Verify(mock => mock.Invoke(It.IsAny<DataParserTypes>()), Times.Once);
        }


        private byte[] GetCorrectTestDataBytes()
        {
            string projectDirectory = Directory
                .GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string path = Path.Combine(projectDirectory, "TestFiles/correctData.xlsx");
            byte[] bytes = File.ReadAllBytes(path);
            return bytes;
        }
    }
}