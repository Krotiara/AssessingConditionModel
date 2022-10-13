using Interfaces;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public interface IPatientDataRepository: IRepository<PatientData>
    {
        Task<Patient> GetPatientDataByIdAsync(int patientMedHistoryNumber, CancellationToken cancellationToken);

        Task<List<PatientData>> GetPatientData(int patientId, DateTime startTimestamp, DateTime endTimestamp);

        Task AddPatientData(PatientData patientData, CancellationToken cancellationToken);

    }
}
