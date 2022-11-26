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
              .UseInMemoryDatabase(databaseName: "test")
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
        public void AddExistedStateMustThrow()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void AddInCorrectStateMustThrow()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public async void GetNotExistedStateMustThrow()
        {
            await Assert.ThrowsAsync<GetAgingStateException>(
                async () => await rep.GetStateAsync(int.MaxValue, DateTime.MaxValue));
        }
    }
}
