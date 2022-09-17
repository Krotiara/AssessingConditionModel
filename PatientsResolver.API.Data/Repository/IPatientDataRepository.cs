using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public interface IPatientDatarepository: IRepository<PatientData>
    {
        Task<Patient> GetPatientDataByIdAsync(long patientMedHistoryNumber, CancellationToken cancellationToken);
    }
}
