using Interfaces;
using Interfaces.Mongo;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Entities.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Store
{
    public class MongoPatientsStore : MongoBaseService<MongoPatient>, IPatientsStore<MongoPatient>
    {
        public MongoPatientsStore(MongoService mongo) : base(mongo, "Patients")
        {
        }

        public Task Delete(string id) => Delete(x => x.Id == id);

        public async Task<MongoPatient> Get(string patientId, string patientAffiliation)
            => await Get(x => x.PatientId == patientId && x.Affiliation == patientAffiliation);

        public async Task<MongoPatient> Get(string id) => await Get(x => x.Id == id);

        public Task<IEnumerable<MongoPatient>> GetAll(string affiliation) 
            => Query(x => x.Affiliation == affiliation);

        public async Task<IPatient> Insert(string patientId, string patientAffiliation)
        {
            var patient = new MongoPatient() { PatientId = patientId, Affiliation = patientAffiliation};
            await Insert(patient);
            return patient;
        }

        public async Task Update(string id, MongoPatient patient)
        {
            await Update(x => x.Id == id)
                       .Set(x => x.Affiliation, patient.Affiliation)
                       .Set(x => x.PatientId, patient.PatientId)
                       .Set(x => x.TreatmentStatus, patient.TreatmentStatus)
                       .Set(x => x.Name, patient.Name)
                       .Set(x => x.Birthday, patient.Birthday)
                       .Set(x => x.Gender, patient.Gender)
                       .Execute();
        }
    }
}
