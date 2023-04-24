using Agents.API.Data.Store;
using Agents.API.Entities;
using Agents.API.Service.Services;
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
       // AgentsDbContext dbContext;
        CancellationToken token;
        //IWebRequester webRequester;

        public UpdatePatientAgentsServiceTests()
        {
            throw new NotImplementedException(); //TODO рефакторинг всвязи с новым api.
            //var options = new DbContextOptionsBuilder<AgentsDbContext>()
            //   .UseInMemoryDatabase(Guid.NewGuid().ToString())
            //   .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            //   .Options;
            //var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            //AgentsDbContext dbContext = new AgentsDbContext(options);
            //token = tokenSource.Token;
            //var dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
            //dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => new AgentsDbContext(options));
            //dbContextFactory = dbFactoryMock.Object;  
        }


        [Fact]
        public async void UpdateExsistedPatientAgentsMustReturnItsCount()
        {
            throw new NotImplementedException(); //TODO рефакторинг всвязи с новым api.
            //double mockAge = new Random().Next(25, 60);
            //double mockBioAge = new Random().Next(30, 50);
            //int mockPatientId = new Random().Next(1, 1000);
            //var webRequesterMock = new Mock<IWebRequester>();
            //webRequesterMock.Setup(x => x.GetResponse<double>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
            //    () => Task.FromResult<double>(mockBioAge));
            //webRequesterMock.Setup(x => x.GetResponse<IList<PatientParameter>>(It.IsAny<string>(), It.IsAny<string>(),
            //    It.IsAny<string>())).Returns(() => GetTestParameters(mockPatientId, mockAge));
            //IWebRequester webRequester = webRequesterMock.Object;
            //IAgentPatientsRepository rep = new AgentPatientsRepository(dbContextFactory, webRequester);
            //UpdatePatientAgentsService service = new UpdatePatientAgentsService(rep);

            //Patient testPatient = GetTestPatient(mockPatientId);
            //AgentPatient aP = await rep.InitAgentPatient(testPatient);
            
            //UpdatePatientsInfo info = new UpdatePatientsInfo() 
            //{ UpdateInfo = new HashSet<(int, DateTime)> { (testPatient.Id, DateTime.Now) } };
            //int updatedCount = await service.UpdatePatientAgents(info);
           
            //Assert.Equal(1, updatedCount);
        }


        [Fact]
        public async void UpdateNotExistedPatientAgentsMustNotCount()
        {
            throw new NotImplementedException(); //TODO рефакторинг всвязи с новым api.
            //double mockAge = new Random().Next(25, 60);
            //double mockBioAge = new Random().Next(30, 50);
            //int mockPatientId = new Random().Next(1, 1000);
            //var webRequesterMock = new Mock<IWebRequester>();
            //webRequesterMock.Setup(x => x.GetResponse<double>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
            //    () => Task.FromResult<double>(mockBioAge));
            //webRequesterMock.Setup(x => x.GetResponse<IList<PatientParameter>>(It.IsAny<string>(), It.IsAny<string>(),
            //    It.IsAny<string>())).Returns(() => GetTestParameters(mockPatientId, mockAge));

            //IWebRequester webRequester = webRequesterMock.Object;
            //IAgentPatientsRepository rep = new AgentPatientsRepository(dbContextFactory, webRequester);
            //UpdatePatientAgentsService service = new UpdatePatientAgentsService(rep);

            //Patient testPatient = GetTestPatient(mockPatientId);

            //UpdatePatientsInfo info = new UpdatePatientsInfo()
            //{ UpdateInfo = new HashSet<(int, DateTime)> { (testPatient.Id, DateTime.Now) } };
            //int updatedCount = await service.UpdatePatientAgents(info);

            //Assert.Equal(0, updatedCount);
        }


        private Task<IList<PatientParameter>> GetTestParameters(int patientId, double mockAge)
        {
            return Task.FromResult<IList<PatientParameter>>(new List<PatientParameter>()
            {
                new PatientParameter(){InfluenceId = 1, NameTextDescription = "возраст", ParameterName = ParameterNames.Age, PatientId = patientId, IsDynamic = false, Timestamp = DateTime.Now, Value = mockAge.ToString() },
                new PatientParameter(){InfluenceId = 1, NameTextDescription = "пол", ParameterName = ParameterNames.Gender, PatientId = patientId, IsDynamic = false, Timestamp = DateTime.Now, Value = "ж" }
            });
        }

        private Patient GetTestPatient(int patientId) => 
            new Patient() { Name = "test", Id = patientId, Gender = GenderEnum.Female, Birthday = DateTime.Today };
    }
}
