using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Service.Store;
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


        public async Task Update(string id, Influence influence)
        {
            await _store.Update(x => x.Id == id)
                .Set(x => x.Affiliation, influence.Affiliation)
                .Set(x => x.InfluenceType, influence.InfluenceType)
                .Set(x => x.StartTimestamp, influence.StartTimestamp)
                .Set(x => x.EndTimestamp, influence.EndTimestamp)
                .Set(x => x.MedicineName, influence.MedicineName)
                .Set(x => x.PatientId, influence.PatientId)
                .Execute();
        }


        public async Task Delete(string id) => await _store.Delete(x => x.Id == id);
    }
}
