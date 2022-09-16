using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PatientsResolver.API.Service.Command
{
    public class UpdatePatientDataCommand: IRequest<IPatientData>
    {
        public IPatientData PatientData { get; set; }
    }
}
