using ASMLib.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities.Events
{
    public class UpdatePatientParametersEvent : Event
    {
        public string PatientId { get; set; }

        public string PatientAffiliation { get; set; }
    }
}
