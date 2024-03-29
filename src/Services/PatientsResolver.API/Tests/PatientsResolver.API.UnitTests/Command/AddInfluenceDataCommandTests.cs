﻿using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Moq;
using PatientsResolver.API.Data;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PatientsResolver.API.UnitTests.Command
{
    public class AddInfluenceDataCommandTests
    {

        Mock<IDbContextFactory<PatientsDataDbContext>> dbContextFactory;
        private PatientsDataDbContext dbContext;

        public AddInfluenceDataCommandTests()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                 .Options;
            dbContextFactory = new Mock<IDbContextFactory<PatientsDataDbContext>>();
            dbContext = new PatientsDataDbContext(options);
            dbContextFactory.Setup(x => x.CreateDbContext())
                .Returns(dbContext);
        }


        [Fact]
        public async void AddExistedDataMustNotBeAdded()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
 
            InfluenceRepository rep = new InfluenceRepository(dbContextFactory.Object);
            int patientId = new Random().Next(1, 1000);
            Influence influence = new Influence()
            {
                InfluenceType = Interfaces.InfluenceTypes.AntiInflammatory,
                MedicineName = "test",
                PatientId = patientId,
                Patient = new Patient() { Id = patientId, 
                    Gender = Interfaces.GenderEnum.Female, Birthday = DateTime.Now, Name = "" },
                StartTimestamp = DateTime.Today,
                EndTimestamp = DateTime.Today,     
            };

            await rep.AddAsync(influence);
                
            AddInfluenceDataCommandHandler handler = new AddInfluenceDataCommandHandler(rep);
            List<Influence> res = await handler.Handle(new AddInfluenceDataCommand() { Data = new List<Influence>() { influence } }, cancellationTokenSource.Token);
            Assert.Empty(res);
            
        }


        [Fact]
        public async Task AddDataWithEmptyFieldsMustThrow()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));        
                List<Influence> testData = GetTestInfluencesWithEmptyFields();
                foreach (Influence influence in testData)
                {
                    if (influence.Patient != null)
                    {
                        await dbContext.Patients.AddAsync(influence.Patient);
                        await dbContext.SaveChangesAsync();
                    }
                    InfluenceRepository rep = new InfluenceRepository(dbContextFactory.Object);
                    AddInfluenceDataCommandHandler handler = new AddInfluenceDataCommandHandler(rep);
                    await Assert.ThrowsAsync<AddInfluenceRangeException>(
                        () => handler.Handle(new AddInfluenceDataCommand() { Data = new List<Influence> { influence } }, cancellationTokenSource.Token));
                
            }
        }


        [Fact]
        public async Task AddInfluenceWherePatientIdNotEqualIdInPatientMustThrow()
        {
            Influence infWithNotEqualPatientIds1 = GetCorrectTestInfluence();
            Influence infWithNotEqualPatientIds2 = GetCorrectTestInfluence();
            infWithNotEqualPatientIds1.PatientId = infWithNotEqualPatientIds1.PatientId + 1;
            infWithNotEqualPatientIds2.Patient.Id = infWithNotEqualPatientIds2.Patient.Id + 1;

            List<Influence> testData = new List<Influence> { infWithNotEqualPatientIds1, infWithNotEqualPatientIds2 };
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            
            InfluenceRepository rep = new InfluenceRepository(dbContextFactory.Object);
            AddInfluenceDataCommandHandler handler = new AddInfluenceDataCommandHandler(rep);
            foreach (Influence influence in testData)
            {
                if (influence.Patient != null)
                {
                    
                    await dbContext.Patients.AddAsync(influence.Patient);
                    await dbContext.SaveChangesAsync();
                    
                }

                await Assert.ThrowsAsync<AddInfluenceRangeException>(
                    () => handler.Handle(new AddInfluenceDataCommand() { Data = new List<Influence> { influence } }, cancellationTokenSource.Token));
            }
        }


        [Fact]
        public async void AddCorrectInfluenceMustBeSuccess()
        {
           
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
              .Options;
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            
            InfluenceRepository rep = new InfluenceRepository(dbContextFactory.Object);
            AddInfluenceDataCommandHandler handler = new AddInfluenceDataCommandHandler(rep);
            Influence inf = GetCorrectTestInfluence();
            PatientsDataDbContext dbContext = dbContextFactory.Object.CreateDbContext();
            
            await dbContext.Patients.AddAsync(inf.Patient);
            await dbContext.SaveChangesAsync();
            

            List<Influence> addedData = await handler.Handle(new AddInfluenceDataCommand() { Data = new List<Influence> { inf } }, cancellationTokenSource.Token);

            
            Influence? added = dbContext.Influences.FirstOrDefault(x => x.InfluenceType == inf.InfluenceType
                                                                                && x.MedicineName == inf.MedicineName
                                                                                && x.PatientId == inf.PatientId
                                                                                && x.StartTimestamp == inf.StartTimestamp
                                                                                && x.EndTimestamp == inf.EndTimestamp);
            Assert.True(addedData.Count == 1);
            Assert.NotNull(added);
            
        }
     

        private List<Influence> GetTestInfluencesWithEmptyFields()
        {
            Influence nullPatient = GetCorrectTestInfluence();
            nullPatient.Patient = null;

            Influence emptyPatient = GetCorrectTestInfluence();
            emptyPatient.PatientId = int.MinValue;

            Influence nullMedicineName = GetCorrectTestInfluence();
            nullMedicineName.MedicineName = null;

            Influence emptyMedicineName = GetCorrectTestInfluence();
            emptyMedicineName.MedicineName = "";

            Influence emptyInfluenceType = GetCorrectTestInfluence();
            emptyInfluenceType.InfluenceType = InfluenceTypes.None;

            Influence emptyStartTimestamp = GetCorrectTestInfluence();
            emptyStartTimestamp.StartTimestamp = default(DateTime);

            Influence emptyEndTimestamp = GetCorrectTestInfluence();
            emptyEndTimestamp.EndTimestamp = default(DateTime);

            return new List<Influence>() { nullPatient, emptyPatient, nullMedicineName, 
                emptyMedicineName, emptyInfluenceType, emptyStartTimestamp, emptyEndTimestamp };
           
        }


        private Influence GetCorrectTestInfluence()
        {
            int medHistoryNumber = new Random().Next(1, 10000);
            var inf = new Influence()
            {
                InfluenceType = Interfaces.InfluenceTypes.Antioxidant,
                MedicineName = "test",
                Patient = new Patient() { Id = medHistoryNumber, Gender = Interfaces.GenderEnum.Female, Birthday = DateTime.Now, Name = "test" },
                StartTimestamp = DateTime.Today,
                EndTimestamp = DateTime.Today,
                PatientId = medHistoryNumber
            };
            inf.StartParameters[ParameterNames.Age] = new PatientParameter() { ParameterName = ParameterNames.Age, Timestamp = DateTime.Today, Value = "40", NameTextDescription = "возраст", IsDynamic = false };
            inf.DynamicParameters[ParameterNames.Age] = new PatientParameter() { ParameterName = ParameterNames.Age, Timestamp = DateTime.Today, Value = "41", NameTextDescription = "возраст", IsDynamic = true };
            return inf;
        }
    }
}
