using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.PatientModel
{
    [Table("GeneralUrineAnalysis")]
    public class GeneralUrineAnalysis
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("WhiteBloodCells")]
        public double WhiteBloodCells { get; set; }

        [Column("FreshRedBloodCells")]
        public double FreshRedBloodCells { get; set; }

        [Column("AlteredRedBloodCells")]
        public double AlteredRedBloodCells { get; set; }

        [Column("Protein")]
        public double Protein { get; set; }
    }
}
