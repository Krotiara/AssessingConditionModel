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
using Interfaces;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Query;
using Moq;
using System.Data.Entity;

namespace PatientsResolver.API.UnitTests.Query
{
    public class GetInfluencesQueryTests
    {
        Mock<IDbContextFactory<PatientsDataDbContext>> dbContextFactory;
        private readonly PatientsDataDbContext dbContext;
        CancellationToken token;

        public GetInfluencesQueryTests()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            dbContextFactory = new Mock<IDbContextFactory<PatientsDataDbContext>>();
            dbContext = new PatientsDataDbContext(options);
            dbContextFactory.Setup(x => x.CreateDbContext())
                .Returns(dbContext);
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
           
            token = cancellationTokenSource.Token;
        }

        [Fact]
        public async void GetInfluencesMustSetPatientProperty()
        {
            Influence testInf = GetCorrectTestInfluence();

            var rep = new InfluenceRepository(dbContextFactory.Object);
            
            await dbContext.Patients.AddAsync(testInf.Patient);
            await dbContext.SaveChangesAsync();
            
            AddInfluenceDataCommandHandler handler = new AddInfluenceDataCommandHandler(rep);
            await handler.Handle(new AddInfluenceDataCommand() { Data = new List<Influence> { testInf } }, token);

            GetInfluencesQueryHandler h = new GetInfluencesQueryHandler(rep);
            var infs = await h.Handle(new GetInfluencesQuery(testInf.StartTimestamp, testInf.EndTimestamp), token);

            Assert.NotNull(infs.First().Patient);
            
        }


        [Fact]
        public async void GetInfluencesMustSetParameters()
        {
            Influence testInf = GetCorrectTestInfluence();
            int startParamsCount = testInf.StartParameters.Count;
            int dynamicParamsCount = testInf.DynamicParameters.Count;
            var rep = new InfluenceRepository(dbContextFactory.Object);
            
            await dbContext.Patients.AddAsync(testInf.Patient);
            await dbContext.SaveChangesAsync();
            
            AddInfluenceDataCommandHandler handler = new AddInfluenceDataCommandHandler(rep);
            await handler.Handle(new AddInfluenceDataCommand() { Data = new List<Influence> { testInf } }, token);

            GetInfluencesQueryHandler h = new GetInfluencesQueryHandler(rep);
            var infs = await h.Handle(new GetInfluencesQuery(testInf.StartTimestamp, testInf.EndTimestamp), token);

            Influence addedTestInnf = infs.First();
            Assert.True(addedTestInnf.StartParameters.Count == startParamsCount);
            Assert.True(addedTestInnf.DynamicParameters.Count == dynamicParamsCount);
        }


        private Influence GetCorrectTestInfluence()
        {
            int medHistoryNumber = new Random().Next(1, 10000);
            var inf = new Influence()
            {
                InfluenceType = Interfaces.InfluenceTypes.Antioxidant,
                MedicineName = "test",
                Patient = new Patient() { MedicalHistoryNumber = medHistoryNumber, Gender = Interfaces.GenderEnum.Female, Birthday = DateTime.Now, Name = "test" },
                StartTimestamp = DateTime.Today,
                EndTimestamp = DateTime.Today,
                PatientId = medHistoryNumber
            };
            inf.StartParameters[ParameterNames.Age] = new PatientParameter() 
            { ParameterName = ParameterNames.Age, Timestamp = DateTime.Today, Value = "40", NameTextDescription = "возраст", IsDynamic = false };
            inf.StartParameters[ParameterNames.Gender] = new PatientParameter() 
            { ParameterName = ParameterNames.Gender, Timestamp = DateTime.Today, Value = "ж", NameTextDescription = "пол", IsDynamic = false };
            inf.DynamicParameters[ParameterNames.Age] = new PatientParameter() 
            { ParameterName = ParameterNames.Age, Timestamp = DateTime.Today, Value = "41", NameTextDescription = "возраст", IsDynamic = true };
            return inf;
        }

    }

}

