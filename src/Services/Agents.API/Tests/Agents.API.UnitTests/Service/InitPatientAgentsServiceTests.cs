using Agents.API.Data.Repository;
using Agents.API.Entities;
using Agents.API.Entities.DynamicAgent;
using Agents.API.Interfaces;
using Agents.API.Service.Services;
using Interfaces;
using Interfaces.DynamicAgent;
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
        CancellationToken token;
        IWebRequester webRequester;
        Mock<IAgentInitSettingsProvider> agentInitSettingsProvider;

        public InitPatientAgentsServiceTests()
        {         
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));          
            token = tokenSource.Token;
            var webRequesterMock = new Mock<IWebRequester>();        
            webRequester = webRequesterMock.Object;
            agentInitSettingsProvider = new Mock<IAgentInitSettingsProvider>();
            agentInitSettingsProvider.Setup(x => x.GetSettingsBy(It.IsAny<AgentType>())).Returns(() => GetTestSettings());
        }


        private IDynamicAgentInitSettings GetTestSettings()
        {           
            Dictionary<string, IAgentState> states = new() { {"Test1", new AgentState("Test1") }, { "Test2", new AgentState("Test2")}};
          
            var sets = new DynamicAgentInitSettings(
                        $"CurrentTest1 = 1\n" +
                        $"CurrentTest2 = 2\n", AgentType.AgingPatient)
            {
                ActionsArgsReplaceDict = new Dictionary<CommonArgs, object>
                        {
                            { CommonArgs.StartDateTime, null },
                            { CommonArgs.EndDateTime, null },
                            { CommonArgs.ObservedId, null }
                        },
                Properties = new Dictionary<string, IProperty>
                        {
                            { "CurrentTest1", new AgentProperty("CurrentTest1", typeof(double)) },
                            { "CurrentTest2", new AgentProperty("CurrentTest2", typeof(double)) }
                        },
                StateDiagram = new StateDiagram(states, async x =>
                {
                    return states["Test1"];
                })
            };
            return sets;
            
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
