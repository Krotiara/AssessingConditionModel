using Microsoft.EntityFrameworkCore;
using Moq;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientsResolver.API.Entities;
using Xunit;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PatientsResolver.API.UnitTests.Data
{
    public class PatientsRepositoryTests
    {

        Mock<IDbContextFactory<PatientsDataDbContext>> dbContextFactory;

        public PatientsRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                 .Options;
            dbContextFactory = new Mock<IDbContextFactory<PatientsDataDbContext>>();
            dbContextFactory.Setup(x => x.CreateDbContext())
                .Returns(new PatientsDataDbContext(options));
        }

        [Fact]
        public async void AddCorrectPatientMustBeSave()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            
            PatientsRepository rep = new PatientsRepository(dbContextFactory.Object);
            Patient testPatient = new Patient() { Name = "test", Id = 000, 
                Gender = Interfaces.GenderEnum.Female, Birthday = DateTime.Now };

            Assert.True(rep.GetAll().Count() == 0);
            await rep.AddAsync(testPatient);
            Assert.True(rep.GetAll().Count() == 1);
            
        }
    }
}
