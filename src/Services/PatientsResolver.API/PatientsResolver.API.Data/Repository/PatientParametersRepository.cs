﻿using Interfaces;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public class PatientParametersRepository: Repository<PatientParameter>
    {
        public PatientParametersRepository(PatientsDataDbContext patientsDataDbContext)
          : base(patientsDataDbContext)
        {

        }


        public async Task<List<PatientParameter>> GetLatestParameters(int patientId, DateTime startTimestamp, DateTime endTimestamp)
        {
            List<PatientParameter> parameters = 
                PatientsDataDbContext.PatientsParameters
                .Where(x => x.PatientId == patientId && x.Timestamp >= startTimestamp && x.Timestamp <= endTimestamp)
                .ToList();
#warning Вынести в отдельный репозиторий метод с установкой этого. Иначе так и будет теряться          
            foreach(PatientParameter parameter in parameters)
                parameter.ParameterName = parameter.NameTextDescription.GetParameterByDescription();

            var groupedParams = parameters.GroupBy(x => x.ParameterName);
            List<PatientParameter> result = new List<PatientParameter>();
            foreach (IGrouping<ParameterNames, PatientParameter> group in groupedParams)
                result.Add(group.OrderBy(x => x.Timestamp).Last());
            return result;
        }
    }
}
