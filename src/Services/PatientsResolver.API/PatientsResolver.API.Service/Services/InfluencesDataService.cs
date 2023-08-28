using PatientsResolver.API.Data.Store;
using PatientsResolver.API.Entities.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Services
{
    public class InfluencesDataService
    {
        private readonly InfluencesStore _store;

        public InfluencesDataService(InfluencesStore store)
        {
            _store = store;
        }


        public async Task<Influence> Get(string id) => await _store.Get(x => x.Id == id);


        public async Task Delete(string id) => await _store.Delete(x => x.Id == id);


        public async Task Insert(Influence influence)
        {
            if (influence.Id != null)
            {
                await _store.Update(x => x.Id == influence.Id)
               .Set(x => x.Affiliation, influence.Affiliation)
               .Set(x => x.InfluenceType, influence.InfluenceType)
               .Set(x => x.StartTimestamp, influence.StartTimestamp)
               .Set(x => x.EndTimestamp, influence.EndTimestamp)
               .Set(x => x.MedicineName, influence.MedicineName)
               .Set(x => x.PatientId, influence.PatientId)
               .Execute();
            }
            else
                await _store.Insert(influence);
        }



        public async Task<IEnumerable<Influence>> Query(string patientId, string affiliation, DateTime start, DateTime end)
        {
            return await _store.Query(x => x.PatientId == patientId
                                        && x.Affiliation == affiliation
                                        && x.StartTimestamp <= end
                                        && x.EndTimestamp >= start);
        }


        public async Task Insert(IEnumerable<Influence> influences)
        {
            foreach (var inf in influences)
                await _store.Insert(inf);
        }
    }
}
