using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class Patient : IPatient
    {
        public Patient()
        {

        }

        public int Id { get ; set ; }
        public string Name { get ; set ; }
        public DateTime Birthday { get ; set ; }

        public GenderEnum Gender { get; set; }
        public TreatmentStatus TreatmentStatus { get; set; }
        public string MedicalOrganization { get; set; }
    }
}
