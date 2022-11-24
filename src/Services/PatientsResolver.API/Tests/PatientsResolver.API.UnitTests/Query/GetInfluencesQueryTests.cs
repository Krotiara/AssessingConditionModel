using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PatientsResolver.API.UnitTests.Query
{
    public class GetInfluencesQueryTests
    {
        PatientsDataDbContext dbContext;
        CancellationToken token;
        InfluenceRepository rep;

        public GetInfluencesQueryTests()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
               .UseInMemoryDatabase(databaseName: "test")
               .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            dbContext = new PatientsDataDbContext(options);
            token = cancellationTokenSource.Token;
            rep = new InfluenceRepository(dbContext);
        }

        [Fact]
        public void GetInfluencesMustSetPatientProperty()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void GetInfluencesWhenPatientNotInDbMustThrow()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void GetInfluencesMustSetParameters()
        {
            throw new NotImplementedException();
        }

    }

}

