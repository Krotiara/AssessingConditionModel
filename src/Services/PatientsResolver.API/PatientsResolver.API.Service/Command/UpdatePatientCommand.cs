using MediatR;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class UpdatePatientCommand: IRequest<Patient>
    {
        public UpdatePatientCommand(Patient patient)
        {
            Patient = patient;
        }

        public Patient Patient { get; }
    }
}
