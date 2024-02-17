using Interfaces;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Store
{
    public interface IPatientsStore<T> where T : IPatient
    {
        public Task<T> Get(string patientId, string patientAffiliation);

        public Task<T> Get(string id);

        public Task<IEnumerable<T>> GetAll(string affiliation = null);

        public Task<T> Insert(string patientId, string patientAffiliation);

        public Task Insert(T p);

        public Task Delete(string id);

        public Task Update(string id, T patient);


    }
}
