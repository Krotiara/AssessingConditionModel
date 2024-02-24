using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.EventBus.Events
{
    public class UpdatePatientEvent : Event
    {
        public string PatientId { get; set; }

        public string PatientAffiliation { get; set; }
    }
}
