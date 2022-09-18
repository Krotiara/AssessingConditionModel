using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API.Service.Services
{
    public interface IParsePatientsDataService
    {
        void ParsePatients(string pathToFile);
    }
}
