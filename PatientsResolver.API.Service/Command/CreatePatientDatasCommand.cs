using Interfaces;
using MediatR;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class CreatePatientDatasCommand: IRequest<List<PatientData>>
    {
        public List<PatientData> PatientDatas { get; set; }

    }
}
