using Agents.API.Data.Repository;
using Agents.API.Entities;
using Agents.API.Service.Services;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Agents.API.UnitTests.Data
{
    public class AgentPatientsRepositoryTests
    {
        //AgentsDbContext dbContext;
        CancellationToken token;

        public AgentPatientsRepositoryTests()
        {
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
           // dbContext = new AgentsDbContext(options);
            token = tokenSource.Token;
        }


        [Fact]
        public async void InitAgentForCorrectPatientMustBeAdded()
        {
            var webRequesterMock = new Mock<IWebRequester>();
            Patient testPatient = GetTestCorrectPatient();

            AgentPatientsRepository rep = new AgentPatientsRepository(dbFactoryMock.Object, webRequesterMock.Object);
            Assert.NotNull(await rep.InitAgentPatient(testPatient));
        }


        [Fact]
        public async void InitAgentForIncorrectPatientMustThrow()
        {
                var dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
                dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => new AgentsDbContext(options));
                var webRequesterMock = new Mock<IWebRequester>();

                IList<Patient> testPatients = GetTestIncorrectPatients();

                AgentPatientsRepository rep = new AgentPatientsRepository(dbFactoryMock.Object, webRequesterMock.Object);
                foreach (Patient testPatient in testPatients)
                {
                    await Assert.ThrowsAsync<InitAgentException>(
                        async () => await rep.InitAgentPatient(testPatient));
                }
            
        }


        [Fact]
        public async void InitAgentMustSetStateDiagram()
        {

            var dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
            dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => new AgentsDbContext(options));
            var webRequesterMock = new Mock<IWebRequester>();
            Patient testPatient = GetTestCorrectPatient();

            AgentPatientsRepository rep = new AgentPatientsRepository(dbFactoryMock.Object, webRequesterMock.Object);
            AgentPatient agent = await rep.InitAgentPatient(testPatient);

            Assert.NotNull(agent.StateDiagram);
            Assert.NotNull(agent.StateDiagram.States);
            Assert.True(agent.StateDiagram.States.Any());

        }


        [Fact]
        public async void GetNotExistedAgentMustThrow()
        {
            double mockBioAge = 40;
            double mockAge = 35;
            AgentBioAgeStates assertRang = AgentBioAgeStates.RangIV;
            Patient testPatient = GetTestCorrectPatient();
            var dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
            dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => new AgentsDbContext(options));
            var webRequesterMock = new Mock<IWebRequester>();
            webRequesterMock.Setup(x => x.GetResponse<double>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
                () => Task.FromResult<double>(mockBioAge));
            webRequesterMock.Setup(x => x.GetResponse<IList<PatientParameter>>(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(() => GetTestParameters(testPatient.MedicalHistoryNumber, mockAge));
            AgentPatientsRepository rep = new AgentPatientsRepository(dbFactoryMock.Object, webRequesterMock.Object);

            await Assert.ThrowsAsync<AgentNotFoundException>(async () 
                => await rep.GetAgentPatient(testPatient.MedicalHistoryNumber));
        }


        [Fact]
        public async void GetExistedAgentMustUpdateState()
        {
            double mockBioAge = 40;
            double mockAge = 35;
            AgentBioAgeStates assertRang = AgentBioAgeStates.RangIV;
            Patient testPatient = GetTestCorrectPatient();
            var dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
            dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => new AgentsDbContext(options));
            var webRequesterMock = new Mock<IWebRequester>();
            webRequesterMock.Setup(x => x.GetResponse<double>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
                () => Task.FromResult<double>(mockBioAge));
            webRequesterMock.Setup(x => x.GetResponse<IList<PatientParameter>>(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(() => GetTestParameters(testPatient.MedicalHistoryNumber, mockAge));
            AgentPatientsRepository rep = new AgentPatientsRepository(dbFactoryMock.Object, webRequesterMock.Object);

            await rep.InitAgentPatient(testPatient);

            AgentPatient agentPatient = await rep.GetAgentPatient(testPatient.MedicalHistoryNumber);

            Assert.True(agentPatient.CurrentAgeRang == assertRang);
            Assert.True(agentPatient.CurrentAge == mockAge);
            Assert.True(agentPatient.CurrentBioAge == mockBioAge);

        }


        [Fact]
        public async void GetAgentPatientWhenThereIsNoSavedAgeParameterMustThrow()
        {
            double mockBioAge = 40;
            AgentBioAgeStates assertRang = AgentBioAgeStates.RangIV;
            Patient testPatient = GetTestCorrectPatient();
            var dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
            dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => new AgentsDbContext(options));
            var webRequesterMock = new Mock<IWebRequester>();
            webRequesterMock.Setup(x => x.GetResponse<double>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
                () => Task.FromResult<double>(mockBioAge));
            webRequesterMock.Setup(x => x.GetResponse<IList<PatientParameter>>(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(() => GetTestParameters(testPatient.MedicalHistoryNumber));
            AgentPatientsRepository rep = new AgentPatientsRepository(dbFactoryMock.Object, webRequesterMock.Object);

            await rep.InitAgentPatient(testPatient);

            await Assert.ThrowsAsync<GetAgentException>(
                async () => await rep.GetAgentPatient(testPatient.MedicalHistoryNumber));
        }


        [Fact]
        public async void UpdateAgentPatientStateWhenThereIsNoSavedAgeParameterMustThrow()
        {
            double mockBioAge = 40;
            AgentBioAgeStates assertRang = AgentBioAgeStates.RangIV;
            Patient testPatient = GetTestCorrectPatient();
            var dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
            dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => new AgentsDbContext(options));
            var webRequesterMock = new Mock<IWebRequester>();
            webRequesterMock.Setup(x => x.GetResponse<double>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
                () => Task.FromResult<double>(mockBioAge));
            webRequesterMock.Setup(x => x.GetResponse<IList<PatientParameter>>(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(() => GetTestParameters(testPatient.MedicalHistoryNumber));
            AgentPatientsRepository rep = new AgentPatientsRepository(dbFactoryMock.Object, webRequesterMock.Object);

            AgentPatient p = await rep.InitAgentPatient(testPatient);

            await Assert.ThrowsAsync<DetermineStateException>(
                async () => await p.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties()));
        }


        private Patient GetTestCorrectPatient()
        {
            int patientId = new Random().Next(1, 10000);
            return new Patient()
            {
                MedicalHistoryNumber = patientId,
                Birthday = DateTime.Today,
                Name = "test",
                Gender = GenderEnum.Male
            };
        }


        private IList<Patient> GetTestIncorrectPatients()
        {
            Patient nullPatient = null;
            Patient emptyGender = GetTestCorrectPatient();
            emptyGender.Gender = GenderEnum.None;
            Patient incorrectId = GetTestCorrectPatient();
            incorrectId.MedicalHistoryNumber = -1;
            return new List<Patient>() { nullPatient, emptyGender, incorrectId };
        }


        private Task<IList<PatientParameter>> GetTestParameters(int patientId, double mockAge)
        {
            return Task.FromResult<IList<PatientParameter>>( new List<PatientParameter>()
            {
                new PatientParameter(){InfluenceId = 1, NameTextDescription = "возраст", ParameterName = ParameterNames.Age, PatientId = patientId, IsDynamic = false, Timestamp = DateTime.Now, Value = mockAge.ToString() },
                new PatientParameter(){InfluenceId = 1, NameTextDescription = "пол", ParameterName = ParameterNames.Gender, PatientId = patientId, IsDynamic = false, Timestamp = DateTime.Now, Value = "ж" }
            });
        }


        private Task<IList<PatientParameter>> GetTestParameters(int patientId)
        {
            return Task.FromResult<IList<PatientParameter>>(new List<PatientParameter>()
            {
                new PatientParameter(){InfluenceId = 1, NameTextDescription = "пол", ParameterName = ParameterNames.Gender, PatientId = patientId, IsDynamic = false, Timestamp = DateTime.Now, Value = "ж" }
            });
        }

    }
}
