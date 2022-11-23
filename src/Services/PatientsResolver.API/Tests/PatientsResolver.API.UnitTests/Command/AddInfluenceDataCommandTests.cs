using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
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
    public class AddInfluenceDataCommandTests
    {

        [Fact]
        public async void AddExistedDataMustNotBeAdded()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
                .UseInMemoryDatabase(databaseName: "test")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            using (PatientsDataDbContext dbContext = new PatientsDataDbContext(options))
            {
                InfluenceRepository rep = new InfluenceRepository(dbContext);
                int patientId = new Random().Next(1, 1000);
                Influence influence = new Influence()
                {
                    InfluenceType = Interfaces.InfluenceTypes.AntiInflammatory,
                    MedicineName = "test",
                    PatientId = patientId,
                    Patient = new Patient() { MedicalHistoryNumber = patientId, 
                        Gender = Interfaces.GenderEnum.Female, Birthday = DateTime.Now, Name = "" },
                    StartTimestamp = DateTime.Today,
                    EndTimestamp = DateTime.Today,     
                };

                await rep.AddAsync(influence);
                
                AddInfluenceDataCommandHandler handler = new AddInfluenceDataCommandHandler(rep);
                List<Influence> res = await handler.Handle(new AddInfluenceDataCommand() { Data = new List<Influence>() { influence } }, cancellationTokenSource.Token);
                Assert.Empty(res);
            }
        }


        [Fact]
        public async Task AddDataWithEmptyFieldsMustThrow()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
               .UseInMemoryDatabase(databaseName: "test")
               .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));        
                List<Influence> testData = GetTestInfluencesWithEmptyFields();
                foreach (Influence influence in testData)
                {
                using (PatientsDataDbContext dbContext = new PatientsDataDbContext(options))
                {
                    InfluenceRepository rep = new InfluenceRepository(dbContext);
                    AddInfluenceDataCommandHandler handler = new AddInfluenceDataCommandHandler(rep);
                    if (influence.Patient != null)
                    {
                        await dbContext.Patients.AddAsync(influence.Patient);
                        await dbContext.SaveChangesAsync();
                    }
                    await Assert.ThrowsAsync<AddInfluenceRangeException>(
                        () => handler.Handle(new AddInfluenceDataCommand() { Data = new List<Influence> { influence } }, cancellationTokenSource.Token));
                }
            }
        }


        [Fact]
        public async Task AddInfluenceWherePatientIdNotEqualIdInPatientMustThrow()
        {
            Influence infWithNotEqualPatientIds1 = GetCorrectTestInfluence();
            Influence infWithNotEqualPatientIds2 = GetCorrectTestInfluence();
            infWithNotEqualPatientIds1.PatientId = infWithNotEqualPatientIds1.PatientId + 1;
            infWithNotEqualPatientIds2.Patient.MedicalHistoryNumber = infWithNotEqualPatientIds2.Patient.MedicalHistoryNumber + 1;

            List<Influence> testData = new List<Influence> { infWithNotEqualPatientIds1, infWithNotEqualPatientIds2 };


            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
               .UseInMemoryDatabase(databaseName: "test")
               .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            using (PatientsDataDbContext dbContext = new PatientsDataDbContext(options))
            {
                InfluenceRepository rep = new InfluenceRepository(dbContext);
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
                Patient = new Patient() { MedicalHistoryNumber = medHistoryNumber, Gender = Interfaces.GenderEnum.Female, Birthday = DateTime.Now, Name = "test" },
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
