using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.PatientModel
{
    [Table("InstrumentalParameters")]
    public class InstrumentalParameters
    {

        public InstrumentalParameters()
        {
            //suitable constructor for entity type for awoid EF error No suitable constructor found for entity type
        }

        [Key]
        [Required]
        public int PatientId { get; set; }
    }
}
