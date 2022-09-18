using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Services
{
    public interface IAddPatientsDataFromSourceService
    {
        public void AddPatientsData(List<PatientData> data);
    }
}
