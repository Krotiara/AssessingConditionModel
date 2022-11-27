using Agents.API.Data.Database;
using Agents.API.Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Agents.API.UnitTests.Service
{
    public class UpdatePatientAgentsServiceTests
    {
        //AgentsDbContext dbContext;
        CancellationToken token;
        IDbContextFactory<AgentsDbContext> dbContextFactory;
        IWebRequester webRequester;

        public UpdatePatientAgentsServiceTests()
        {
            var options = new DbContextOptionsBuilder<AgentsDbContext>()
               .UseInMemoryDatabase(databaseName: "test")
               .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            //dbContext = new AgentsDbContext(options);
            token = tokenSource.Token;
            var dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
            dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => new AgentsDbContext(options));
            dbContextFactory = dbFactoryMock.Object;  
        }


        [Fact]
        public void UpdateExsistedPatientAgentsMustReturnItsCount()
        {
            throw new NotImplementedException();
            //var webRequesterMock = new Mock<IWebRequester>();
            //webRequesterMock.Setup(x => x.GetResponse<double>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
            //    () => Task.FromResult<double>(mockBioAge));
            //webRequesterMock.Setup(x => x.GetResponse<IList<PatientParameter>>(It.IsAny<string>(), It.IsAny<string>(),
            //    It.IsAny<string>())).Returns(() => GetTestParameters(testPatient.MedicalHistoryNumber));
            //webRequester = webRequesterMock.Object;
        }


        [Fact]
        public void UpdateNotExistedPatientAgentsMustNotCount()
        {
            throw new NotImplementedException();
        }


        private Task<IList<PatientParameter>> GetTestParameters(int patientId, double mockAge)
        {
            return Task.FromResult<IList<PatientParameter>>(new List<PatientParameter>()
            {
                new PatientParameter(){InfluenceId = 1, NameTextDescription = "возраст", ParameterName = ParameterNames.Age, PatientId = patientId, IsDynamic = false, Timestamp = DateTime.Now, Value = mockAge.ToString() },
                new PatientParameter(){InfluenceId = 1, NameTextDescription = "пол", ParameterName = ParameterNames.Gender, PatientId = patientId, IsDynamic = false, Timestamp = DateTime.Now, Value = "ж" }
            });
        }
    }
}
