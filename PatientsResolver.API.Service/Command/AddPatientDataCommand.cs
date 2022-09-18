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
    public class AddPatientDataCommand: IRequest<List<PatientData>>
    {
        public List<PatientData> Data { get; set; }
    }
}
