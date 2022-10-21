using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Services
{
    public interface IAddInfluencesDataFromSourceService
    {
        public void AddInfluencesData(List<Influence> data);
    }
}
