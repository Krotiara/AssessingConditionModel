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

                Influence influence = new Influence()
                {
                    InfluenceType = Interfaces.InfluenceTypes.AntiInflammatory,
                    MedicineName = "test",
                    Patient = new Patient() { MedicalHistoryNumber = 000, Gender = Interfaces.GenderEnum.Female, Birthday = DateTime.Now, Name = "" },
                    StartTimestamp = DateTime.MinValue,
                    EndTimestamp = DateTime.MaxValue,
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
#warning Тест не проходит из-за 1) Ошибок в тестовых данных и 2) В самом коде не достаточно отлова частных случаев.
                    if (influence.Patient != null)
                    {
                        try
                        {
                            await dbContext.Patients.AddAsync(influence.Patient);
                            await dbContext.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            var a = 1;
                        }
                    }
                    await Assert.ThrowsAsync<AddInfluenceRangeException>(
                        () => handler.Handle(new AddInfluenceDataCommand() { Data = new List<Influence> { influence } }, cancellationTokenSource.Token));
                }
            }
        }


       

        private List<Influence> GetTestInfluencesWithEmptyFields()
        {
            Influence nullPatient = GetCorrectTestInfluence(1);
            nullPatient.Patient = null;

            Influence emptyPatient = GetCorrectTestInfluence(2);
            emptyPatient.PatientId = int.MinValue;

            Influence nullMedicineName = GetCorrectTestInfluence(3);
            nullMedicineName.MedicineName = null;

            Influence emptyMedicineName = GetCorrectTestInfluence(4);
            emptyMedicineName.MedicineName = "";

            Influence emptyInfluenceType = GetCorrectTestInfluence(5);
            emptyInfluenceType.InfluenceType = InfluenceTypes.None;

            Influence emptyStartTimestamp = GetCorrectTestInfluence(6);
            emptyStartTimestamp.StartTimestamp = default(DateTime);

            Influence emptyEndTimestamp = GetCorrectTestInfluence(7);
            emptyEndTimestamp.EndTimestamp = default(DateTime);

            return new List<Influence>() { nullPatient, emptyPatient, nullMedicineName, 
                emptyMedicineName, emptyInfluenceType, emptyStartTimestamp, emptyEndTimestamp };
           
        }


        private Influence GetCorrectTestInfluence(int medHistoryNumber = 100)
        {           
            var inf = new Influence()
            {
                InfluenceType = Interfaces.InfluenceTypes.Antioxidant,
                MedicineName = "test",
                Patient = new Patient() { MedicalHistoryNumber = medHistoryNumber, Gender = Interfaces.GenderEnum.Female, Birthday = DateTime.Now, Name = "test" },
                StartTimestamp = DateTime.MinValue,
                EndTimestamp = DateTime.MaxValue,
                PatientId = medHistoryNumber
            };
            inf.StartParameters[ParameterNames.Age] = new PatientParameter() { ParameterName = ParameterNames.Age, Timestamp = DateTime.MinValue, Value = "40", NameTextDescription = "возраст", IsDynamic = false };
            inf.DynamicParameters[ParameterNames.Age] = new PatientParameter() { ParameterName = ParameterNames.Age, Timestamp = DateTime.MaxValue, Value = "41", NameTextDescription = "возраст", IsDynamic = true };
            return inf;
        }
    }
}
