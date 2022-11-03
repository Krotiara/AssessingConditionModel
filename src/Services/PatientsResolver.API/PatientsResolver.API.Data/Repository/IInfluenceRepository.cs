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
        Task<List<Influence>> GetPatientInfluences(int patientId, DateTime startTimestamp, DateTime endTimestamp);

        Task<List<Influence>> GetInfluences(DateTime startTimestamp, DateTime endTimestamp);

        Task<bool> AddPatientInluence(Influence inluence, CancellationToken cancellationToken);
    }
}
