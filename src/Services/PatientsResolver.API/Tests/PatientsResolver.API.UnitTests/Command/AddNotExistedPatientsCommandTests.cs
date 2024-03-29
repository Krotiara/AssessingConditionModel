﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
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
    public class AddNotExistedPatientsCommandTests
    {

        Mock<IDbContextFactory<PatientsDataDbContext>> dbContextFactory;

        public AddNotExistedPatientsCommandTests()
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
        public async void AddExistedPatientMustNotBeAdded()
        {   
           
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
          
            PatientsRepository rep = new PatientsRepository(dbContextFactory.Object);
            AddPatientCommandHandler addPatientCommandHandler = new AddPatientCommandHandler(rep);
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsAny<AddPatientCommand>(),
                It.IsAny<CancellationToken>()))
                .Returns(addPatientCommandHandler.Handle);


            Patient testPatient = new Patient() { Name = "test", Id = 000, 
                Gender = Interfaces.GenderEnum.Female, Birthday = DateTime.Now };
            await rep.AddAsync(testPatient);

            AddNotExistedPatientsCommandHandler handler = new AddNotExistedPatientsCommandHandler(rep, mockMediator.Object);
            IList<Patient> addedPatients = await handler.Handle(new AddNotExistedPatientsCommand() 
            { Patients = new List<Patient> { testPatient } }, cancellationTokenSource.Token);

            Assert.Equal(0, addedPatients.Count);
            
        }

        [Fact]
        public async void CorrectPatientsMustBeAdded()
        {
            List<Patient> patients = new List<Patient>() { GetTestCorrectPatient(),
                GetTestCorrectPatient(), GetTestCorrectPatient() };

            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                 .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            
            PatientsRepository rep = new PatientsRepository(dbContextFactory.Object);
            AddPatientCommandHandler addPatientCommandHandler = new AddPatientCommandHandler(rep);
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsAny<AddPatientCommand>(),
                It.IsAny<CancellationToken>()))
                .Returns(addPatientCommandHandler.Handle);
            AddNotExistedPatientsCommandHandler handler = new AddNotExistedPatientsCommandHandler(rep, mockMediator.Object);


            IList<Patient> addedPatients = await handler.Handle(
                new AddNotExistedPatientsCommand() { Patients = patients }, cancellationTokenSource.Token);

            Assert.Equal(patients.Count, addedPatients.Count);
                      
        }


        private Patient GetTestCorrectPatient() => new Patient()
        {
            Id = new Random().Next(1,1000),
            Name = "Test name",
            Gender = Interfaces.GenderEnum.Female,
            Birthday = DateTime.Today
        };


        [Fact]
        public async void AddPatientWithEmptFieldsMustThrow()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                 .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            Patient emptyHistory = new Patient() { Name = "test", Gender = Interfaces.GenderEnum.Female, Birthday = DateTime.Now };
            Patient emptyGender = new Patient() { Name = "test", Birthday = DateTime.Now, Id = 000};
            Patient emptyBirthday = new Patient() { Name = "test", Gender = Interfaces.GenderEnum.Female, Id = 000 };
            
            PatientsRepository rep = new PatientsRepository(dbContextFactory.Object);
            AddPatientCommandHandler addPatientCommandHandler = new AddPatientCommandHandler(rep);

            Mock<IMediator> mockMediator = new Mock<IMediator>();  
            mockMediator.Setup(m => m.Send(It.IsAny<AddPatientCommand>(), 
                It.IsAny<CancellationToken>()))
                .Returns(addPatientCommandHandler.Handle);

            AddNotExistedPatientsCommandHandler handler = new AddNotExistedPatientsCommandHandler(rep, mockMediator.Object);
            await Assert.ThrowsAsync<AddPatientsRangeException>(
                () => handler.Handle(new AddNotExistedPatientsCommand() 
                { Patients = new List<Patient> { emptyHistory } }, cancellationTokenSource.Token));
            await Assert.ThrowsAsync<AddPatientsRangeException>(
                () => handler.Handle(new AddNotExistedPatientsCommand() 
                { Patients = new List<Patient> { emptyGender } }, cancellationTokenSource.Token));
            await Assert.ThrowsAsync<AddPatientsRangeException>(
                () => handler.Handle(new AddNotExistedPatientsCommand() 
                { Patients = new List<Patient> { emptyBirthday } }, cancellationTokenSource.Token));
            
        }
    }
}
