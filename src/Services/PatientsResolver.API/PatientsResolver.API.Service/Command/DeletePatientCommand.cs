using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class DeletePatientCommand: IRequest<bool>
    {
        public int PatientId { get; set; }

        public DeletePatientCommand(int patientId)
        {
            PatientId = patientId;
        }
    }
}
