﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using PatientsResolver.API.Data;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PatientsResolver.API.UnitTests.Command
{
    public class AddPatientCommandTests
    {
        Mock<IDbContextFactory<PatientsDataDbContext>> dbContextFactory;

        public AddPatientCommandTests()
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
        public async void CorrectPatientMustBeSave()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            
            PatientsRepository p = new PatientsRepository(dbContextFactory.Object);
            int testMedNumber = new Random().Next(1,1000);
            Patient testPatient = new Patient()
            {
                Id = testMedNumber,
                Name = "",
                Gender = Interfaces.GenderEnum.Female,
                Birthday = DateTime.Today
            };
            AddPatientCommandHandler handler = new AddPatientCommandHandler(p);
            Assert.True(await handler.Handle(
                new AddPatientCommand(){ Patient = testPatient }, cancellationTokenSource.Token));
            Assert.True(p.GetPatientBy(testMedNumber,"test") != null); 
        }


        [Fact]
        public async void AddPatientWithEmptyFieldsMustThrow()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            
            PatientsRepository p = new PatientsRepository(dbContextFactory.Object);
            AddPatientCommandHandler handler = new AddPatientCommandHandler(p);

            Patient emptyHistory = GetTestCorrectPatient();
            emptyHistory.Id = int.MinValue;
            Patient emptyGender = GetTestCorrectPatient();
            emptyGender.Gender = Interfaces.GenderEnum.None;
            Patient emptyBirthday = GetTestCorrectPatient();
            emptyBirthday.Birthday = default(DateTime);
            Patient nullName = GetTestCorrectPatient();
            nullName.Name = null;

            List<Patient> testPatients = new List<Patient> { emptyHistory, emptyGender, emptyBirthday, nullName};
            foreach (var testPatient in testPatients)
                await Assert.ThrowsAsync<AddPatientException>(() => handler.Handle(new AddPatientCommand() 
                { Patient = testPatient}, cancellationTokenSource.Token));
            
        }


        [Fact]
        public async void PatientWithEmptyNameButWithOtherCorrectDataMustBeAdded()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            
            PatientsRepository p = new PatientsRepository(dbContextFactory.Object);
            AddPatientCommandHandler handler = new AddPatientCommandHandler(p);
            Patient emptyName = GetTestCorrectPatient();
            int testMedNumber = emptyName.Id;
            emptyName.Name = "";
            await handler.Handle(
                new AddPatientCommand() { Patient = emptyName }, cancellationTokenSource.Token);
            Assert.True(p.GetPatientBy(testMedNumber,"test") != null);
                        
        }


        [Fact]
        public async void AddExistedPatientMustThrow()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
              .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            
            PatientsRepository p = new PatientsRepository(dbContextFactory.Object);
            AddPatientCommandHandler handler = new AddPatientCommandHandler(p);
            Patient testPatient = GetTestCorrectPatient();
            int testMedNumber = testPatient.Id;
            await handler.Handle(
                new AddPatientCommand() { Patient = testPatient }, cancellationTokenSource.Token);

            await Assert.ThrowsAsync<AddPatientException>(() => handler.Handle(new AddPatientCommand()
            { Patient = testPatient }, cancellationTokenSource.Token));
            
        }


        private Patient GetTestCorrectPatient() => new Patient()
        {
            Id = new Random().Next(1, 1000),
            Name = "Test name",
            Gender = Interfaces.GenderEnum.Female,
            Birthday = DateTime.Today
        };
    }
}
