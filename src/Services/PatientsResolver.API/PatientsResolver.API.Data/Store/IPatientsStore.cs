using Interfaces;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Store
{
    public interface IPatientsStore
    {
        public Task<IPatient> Get(string patientId, string patientAffiliation);

        public Task<IPatient> Get(string id);

        public Task<IEnumerable<IPatient>> GetAll(string affiliation = null);

        public Task<IPatient> Insert(string patientId, string patientAffiliation);

        public Task Insert(IPatient p);

        public Task Delete(string id);

        public Task Update(string id, IPatient patient);


    }
}
