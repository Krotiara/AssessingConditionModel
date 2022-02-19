using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    public class Patient
    {
       
        public int Id { get; set; }

        public string Name { get; set; }

        public ClinicalParameters ClinicalParameters { get; set; }

        public FunctionalParameters FunctionalParameters { get; set; }

        public InstrumentalParameters InstrumentalParameters { get; set; }

        public ParametersNorms ParametersNorms { get; set; }

       
        public Patient(string name)
        {
            Name = name;
            
        }
    }
}
