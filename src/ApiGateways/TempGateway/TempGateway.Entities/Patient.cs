using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempGateway.Entities
{
    public class Patient : IPatient
    {
        public int Id { get ; set ; }
        public string Name { get ; set ; }
        public DateTime Birthday { get ; set ; }
        public int MedicalHistoryNumber { get ; set ; }

        public GenderEnum Gender { get; set; }
        public TreatmentType TreatmentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
