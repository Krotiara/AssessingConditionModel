using ASMLib;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities
{
    public class PatientInfo
    {
        public IPatient Patient { get; set; }

        public IPatientMeta Meta { get; set; }
    }
}
