using AssessingConditionModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using Xunit;

namespace AssessingConditionModelTests
{
    public class DbTests
    {
        private readonly Microsoft.EntityFrameworkCore.DbContextOptions<PatientsContext> dbContextOptions;

        public DbTests()
        {
            dbContextOptions = new DbContextOptionsBuilder<PatientsContext>()
               .UseInMemoryDatabase("PatientsDB")
               .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)) //Transactions are not supported by the in-memory store.
               .Options;
        }


        [Fact]
        public void DbConnectionTest()
        {
            using var context = new PatientsContext(dbContextOptions);
            Assert.True(context.Database.CanConnect()); 
        }
    }
}
