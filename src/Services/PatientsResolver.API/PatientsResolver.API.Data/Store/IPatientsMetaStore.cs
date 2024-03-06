using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Data.Store
{
    public interface IPatientsMetaStore
    {
        public Task<IPatientMeta> Get(string patientId);

        public Task<IPatientMeta> Insert(IPatientMeta meta);

        public Task Delete(string patientId);
    }
}
