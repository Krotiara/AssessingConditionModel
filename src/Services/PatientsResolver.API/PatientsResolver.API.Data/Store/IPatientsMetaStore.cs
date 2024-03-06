using ASMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Store
{
    public interface IPatientsMetaStore
    {
        public Task<IPatientMeta> Get(string patientId);

        public Task<IPatientMeta> Insert(IPatientMeta meta);

        public Task Delete(string patientId);
    }
}
