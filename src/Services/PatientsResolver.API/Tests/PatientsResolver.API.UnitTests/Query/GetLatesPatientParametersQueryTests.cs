using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json.Linq;
using PatientsResolver.API.Data;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PatientsResolver.API.UnitTests.Query
{
    public class GetLatesPatientParametersQueryTests
    {
        PatientsDataDbContext dbContext;
        CancellationToken token;
        InfluenceRepository rep;
        PatientParametersRepository patientParamsRep;

        public GetLatesPatientParametersQueryTests()
        {
            var options = new DbContextOptionsBuilder<PatientsDataDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;
            // set delay time after which the CancellationToken will be canceled
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            dbContext = new PatientsDataDbContext(options);
            token = cancellationTokenSource.Token;
            rep = new InfluenceRepository(dbContext);
            patientParamsRep = new PatientParametersRepository(dbContext);
        }

        [Fact]
        public async void GetLatesPatientParametersMustSetInternalTypeProperty()
        {
            using(dbContext)
            {
                Influence testInf = GetCorrectTestInfluence();
                AddInfluenceDataCommandHandler handler = new AddInfluenceDataCommandHandler(rep);
                await dbContext.Patients.AddAsync(testInf.Patient);
                await dbContext.SaveChangesAsync();
                List<Influence> addedData = await handler.Handle(new AddInfluenceDataCommand() { Data = new List<Influence> { testInf } }, token);

                GetLatesPatientParametersQueryHandler handlerParams = new GetLatesPatientParametersQueryHandler(patientParamsRep);
                List<PatientParameter> parameters = await handlerParams.Handle(
                    new GetLatesPatientParametersQuery() { PatientId = testInf.PatientId, StartTimestamp = testInf.StartTimestamp, EndTimestamp = testInf.EndTimestamp },token);

                foreach (var parameter in parameters)
                    Assert.True(parameter.ParameterName != ParameterNames.None);
            }
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
            inf.StartParameters[ParameterNames.Gender] = new PatientParameter() { ParameterName = ParameterNames.Gender, Timestamp = DateTime.Today, Value = "ж", NameTextDescription = "пол", IsDynamic = false };
            inf.DynamicParameters[ParameterNames.Age] = new PatientParameter() { ParameterName = ParameterNames.Age, Timestamp = DateTime.Today, Value = "41", NameTextDescription = "возраст", IsDynamic = true };
            return inf;
        }
    }
}
