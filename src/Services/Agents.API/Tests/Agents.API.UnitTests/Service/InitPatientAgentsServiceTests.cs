using Agents.API.Data.Database;
using Agents.API.Data.Repository;
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
    public class InitPatientAgentsServiceTests
    {
        AgentsDbContext dbContext;
        CancellationToken token;

        public InitPatientAgentsServiceTests()
        {
            var options = new DbContextOptionsBuilder<AgentsDbContext>()
                .UseInMemoryDatabase(databaseName: "test")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            dbContext = new AgentsDbContext(options);
            token = tokenSource.Token;    
        }

        [Fact]
        public async void InitAgentForCorrectPatientMustBeReturn()
        {
            using(dbContext)
            {
                var dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
                dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => dbContext);
                var webRequesterMock = new Mock<IWebRequester>();
                Patient testPatient = GetTestCorrectPatient();

                AgentPatientsRepository rep = new AgentPatientsRepository(dbFactoryMock.Object, webRequesterMock.Object);
                InitPatientAgentsService service = new InitPatientAgentsService(rep);

                Assert.NotEmpty(await service.InitPatientAgentsAsync(new List<IPatient> { testPatient }));
            }
        }


        [Fact]
        public async void InitAgentForIncorrectPatientMustThrow()
        {
            using (dbContext)
            {
                var dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
                dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => dbContext);
                var webRequesterMock = new Mock<IWebRequester>();

                IList<Patient> testPatients = GetTestIncorrectPatients();

                AgentPatientsRepository rep = new AgentPatientsRepository(dbFactoryMock.Object, webRequesterMock.Object);
                InitPatientAgentsService service = new InitPatientAgentsService(rep);
                foreach (Patient testPatient in testPatients)
                {
                   
                    await Assert.ThrowsAsync<InitAgentsRangeException>(
                        async () => await service.InitPatientAgentsAsync(new List<IPatient> { testPatient }));
                }
            }
        }


        [Fact]
        public async void InitAgentMustSetStateDiagram()
        {
            using (dbContext)
            {
                var dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
                dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => dbContext);
                var webRequesterMock = new Mock<IWebRequester>();
                Patient testPatient = GetTestCorrectPatient();

                AgentPatientsRepository rep = new AgentPatientsRepository(dbFactoryMock.Object, webRequesterMock.Object);
                InitPatientAgentsService service = new InitPatientAgentsService(rep);

                var agents = await service.InitPatientAgentsAsync(new List<IPatient> { testPatient });
                AgentPatient agent = agents[0];

                Assert.NotNull(agent.StateDiagram);
                Assert.NotNull(agent.StateDiagram.States);
                Assert.True(agent.StateDiagram.States.Any());
            }
        }


        private Patient GetTestCorrectPatient()
        {
            int patientId = new Random().Next(1, 10000);
            return new Patient() { MedicalHistoryNumber = patientId, 
                Birthday = DateTime.Today, Name = "test", Gender = GenderEnum.Male };
        }


        private IList<Patient> GetTestIncorrectPatients()
        {
            Patient nullPatient = null;
            Patient emptyGender = GetTestCorrectPatient();
            emptyGender.Gender = GenderEnum.None;
            Patient incorrectId = GetTestCorrectPatient();
            incorrectId.MedicalHistoryNumber = -1;
            return new List<Patient>(){ nullPatient, emptyGender, incorrectId };
        }
       
    }
}
