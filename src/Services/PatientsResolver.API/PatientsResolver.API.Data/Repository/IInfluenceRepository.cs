using Interfaces;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public interface IInfluenceRepository: IRepository<Influence>
    {
        Task<List<Influence>> GetPatientInfluences(int patientId, DateTime startTimestamp, DateTime endTimestamp, bool includeParams = true);

        Task<List<Influence>> GetInfluences(DateTime startTimestamp, DateTime endTimestamp, bool includeParams = true);

        Task<bool> AddPatientInluence(Influence inluence, CancellationToken cancellationToken);

        Task<Influence?> GetPatientInfluence(int inluenceId);

        Task<Influence> UpdateInfluence(Influence influence, CancellationToken cancellationToken);

        Task<bool> DeleteInfluence(int influenceId, CancellationToken cancellationToken);
    }
}
