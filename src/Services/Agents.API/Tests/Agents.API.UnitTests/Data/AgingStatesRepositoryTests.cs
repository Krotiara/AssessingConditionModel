using Agents.API.Data.Database;
using Agents.API.Data.Repository;
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

namespace Agents.API.UnitTests.Data
{
    public class AgingStatesRepositoryTests
    {
        CancellationToken token;
        DbContextOptions<AgentsDbContext> options;
        Mock<IDbContextFactory<AgentsDbContext>> dbFactoryMock;
        IAgingStatesRepository rep;

        public AgingStatesRepositoryTests()
        {
            options = new DbContextOptionsBuilder<AgentsDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
              .Options;
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            // dbContext = new AgentsDbContext(options);
            token = tokenSource.Token;
            dbFactoryMock = new Mock<IDbContextFactory<AgentsDbContext>>();
            dbFactoryMock.Setup(x => x.CreateDbContext()).Returns(() => new AgentsDbContext(options));
            var webRequesterMock = new Mock<IWebRequester>();
            rep = new AgentPatientsRepository(dbFactoryMock.Object, webRequesterMock.Object);
        }

        [Fact]
        public async void AddExistedStateWithoutOverrideMustThrow()
        {
            int patientId = new Random().Next(1, 1000);
            DateTime timeStamp = DateTime.Today;
            AgingState testState = new AgingState()
            {
                PatientId = patientId,
                BioAge = 40,
                Age = 40,
                Timestamp = timeStamp,
                BioAgeState = AgentBioAgeStates.RangIII
            };

            await rep.AddState(testState, false);

            await Assert.ThrowsAsync<AddAgingStateException>(async () => await rep.AddState(testState, false));
        }


        [Fact]
        public async void AddIncorrectStateMustThrow()
        {
            List<AgingState> incorrectTestStates = GetIncorrectAgingStates();
            foreach(AgingState state in incorrectTestStates)
                await Assert.ThrowsAsync<AddAgingStateException>(async () => await rep.AddState(state, false));
        }


        [Fact]
        public async void GetNotExistedStateMustThrow()
        {
            await Assert.ThrowsAsync<GetAgingStateException>(
                async () => await rep.GetStateAsync(int.MaxValue, DateTime.MaxValue));
        }


        private List<AgingState> GetIncorrectAgingStates()
        {
            AgingState inCorrectId = GetAgingState(-1, DateTime.Today);
            AgingState inCorrectDate = GetAgingState(new Random().Next(1, 1000), default(DateTime));
            AgingState incorrectAge = GetAgingState(new Random().Next(1, 1000), DateTime.Today);
            incorrectAge.Age = -1;
            AgingState incorrectBioAge = GetAgingState(new Random().Next(1, 1000), DateTime.Today);
            incorrectBioAge.BioAge = -1;
            return new List<AgingState> { inCorrectId, inCorrectDate, incorrectAge, incorrectBioAge };
        }


        private AgingState GetAgingState(int patientId, DateTime timeStamp) => new AgingState()
        {
            PatientId = patientId,
            BioAge = 40,
            Age = 40,
            Timestamp = timeStamp,
            BioAgeState = AgentBioAgeStates.RangIII
        };


       
    }
}
