using Agents.API.Data.Repository;
using Agents.API.Entities;
using Agents.API.Interfaces;
using Agents.API.Service.Services;
using Interfaces;
using Interfaces.DynamicAgent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Agents.API.UnitTests.Service
{
    public class InitPatientAgentsServiceTests
    {       
        CancellationToken token;
        IWebRequester webRequester;

        public InitPatientAgentsServiceTests()
        {         
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));          
            token = tokenSource.Token;
            var webRequesterMock = new Mock<IWebRequester>();        
            webRequester = webRequesterMock.Object;
        }


        [Fact]
        public async void InitAgentForCorrectPatientMustBeReturn()
        {
            throw new NotImplementedException(); //TODO доработать в связи с правкой API
            //Patient testPatient = GetTestCorrectPatient();
            //IDynamicAgentsRepository rep = new DynamicAgentsRepository(webRequester);
            
            //InitPatientAgentsService service = new InitPatientAgentsService(rep, agentInitSettingsProvider.Object);

            //Assert.NotEmpty(await service.InitPatientAgentsAsync(new List<(IPatient,AgentType)> { (testPatient, AgentType.AgingPatient) }));

        }


        [Fact]
        public async void InitAgentForIncorrectPatientMustThrow()
        {
            throw new NotImplementedException(); //TODO доработать в связи с правкой API
            //IList<Patient> testPatients = GetTestIncorrectPatients();
            //    IDynamicAgentsRepository rep = new DynamicAgentsRepository(webRequester);
            //    InitPatientAgentsService service = new InitPatientAgentsService(rep, agentInitSettingsProvider.Object);
            //    foreach (Patient testPatient in testPatients)
            //    {
            //        await Assert.ThrowsAsync<InitAgentsRangeException>(
            //            async () => await service.InitPatientAgentsAsync(new List<(IPatient, AgentType)> { (testPatient, AgentType.AgingPatient) }));
            //}
            
        }


        [Fact]
        public async void InitAgentMustSetStateDiagram()
        {
            throw new NotImplementedException(); //TODO доработать в связи с правкой API
            //Patient testPatient = GetTestCorrectPatient();

            //IDynamicAgentsRepository rep = new DynamicAgentsRepository(webRequester);
            //InitPatientAgentsService service = new InitPatientAgentsService(rep, agentInitSettingsProvider.Object);

            //var agents = await service.InitPatientAgentsAsync(new List<(IPatient, AgentType)> { (testPatient, AgentType.AgingPatient) });
            //IDynamicAgent agent = agents[0];

            //Assert.NotNull(agent.Settings.StateDiagram);
            //Assert.NotNull(agent.Settings.StateDiagram.States);
            //Assert.True(agent.Settings.StateDiagram.States.Any());    
        }


        private Patient GetTestCorrectPatient()
        {
            int patientId = new Random().Next(1, 10000);
            return new Patient()
            {
                Id = patientId,
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
            incorrectId.Id = -1;
            return new List<Patient>(){ nullPatient, emptyGender, incorrectId };
        }
       
    }
}
