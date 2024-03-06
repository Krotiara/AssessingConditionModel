using ASMLib;
using Interfaces.Mongo;
using PatientsResolver.API.Entities.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Store
{
    public class MongoPatientsMetaStore : MongoBaseService<MongoPatientMeta>, IPatientsMetaStore
    {
        public MongoPatientsMetaStore(MongoService mongo) : base(mongo, "PatientsMeta")
        {
        }

        public Task Delete(string patientId) => Delete(x => x.PatientId == patientId);

        public async Task<IPatientMeta> Get(string patientId) => await Get(x => x.PatientId == patientId);

        public async Task<IPatientMeta> Insert(IPatientMeta meta)
        {
            var dbM = await Get(meta.PatientId);
            if (dbM != null)
            {
                await Update(x => x.PatientId == dbM.PatientId)
                    .Set(x => x.InputParametersTimestamps, meta.InputParametersTimestamps)
                    .Execute();
                dbM.InputParametersTimestamps = meta.InputParametersTimestamps;
                return dbM;
            }
            else
            {
                MongoPatientMeta m = new ()
                {
                    PatientId = meta.PatientId,
                    InputParametersTimestamps = meta.InputParametersTimestamps
                };
                await base.Insert(m);
                return m;
            }
        }
    }
}
