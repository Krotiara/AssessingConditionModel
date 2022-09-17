using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Services
{
    public interface IPatientDataUpdateService
    {
        void UpdatePatientData(IPatientData patientData);


    }
}
