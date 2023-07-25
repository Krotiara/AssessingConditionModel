using Interfaces;
using PatientsResolver.API.Entities.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public interface IInfluenceRepository: IRepository<Influence>
    {
        Task<List<Influence>> GetPatientInfluences(int patientId, string medicalOrganization, 
            DateTime startTimestamp, DateTime endTimestamp, bool includeParams = true);

        Task<List<Influence>> GetInfluences(DateTime startTimestamp, DateTime endTimestamp, bool includeParams = true);

        Task<bool> AddPatientInluence(Influence inluence, CancellationToken cancellationToken);

        Task<Influence?> GetPatientInfluence(int inluenceId, string medicalOrganization);

        Task<Influence> UpdateInfluence(Influence influence, CancellationToken cancellationToken);

        Task<bool> DeleteInfluence(int influenceId, string medicalOrganization, CancellationToken cancellationToken);
    }
}
