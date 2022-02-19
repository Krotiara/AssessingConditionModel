using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    [Table("Patients")]
    public class Patient
    {
       
        public string Name { get; set; }

        [Key]
        [Column("MedicalHistoryNumber")]
        [Required]
        public int MedicalHistoryNumber { get; set; }


        public ClinicalParameters ClinicalParameters { get; set; }

        public FunctionalParameters FunctionalParameters { get; set; }

        public InstrumentalParameters InstrumentalParameters { get; set; }

        public ParametersNorms ParametersNorms { get; set; }

       
        public Patient(string name)
        {
            Name = name;
            
        }

        public Patient()
        {
            //suitable constructor for entity type for awoid EF error No suitable constructor found for entity type
        }
    }
}
