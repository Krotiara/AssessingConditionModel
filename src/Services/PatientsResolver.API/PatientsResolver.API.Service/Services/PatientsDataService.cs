using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Service.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Services
{
    public class PatientsDataService
    {
        private readonly PatientsStore _patientsStore;

        public PatientsDataService(PatientsStore patientsStore)
        {
            _patientsStore = patientsStore;
        }

        public async Task Insert(IEnumerable<Influence> influences)
        {
            throw new NotImplementedException();
        }


        public async Task Insert(IEnumerable<Patient> patients)
        {
            throw new NotImplementedException();
        }
    }
}
