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
        [Display(Name = "Лейкоциты")]
        public double WhiteBloodCells { get; set; }

        [Column("FreshRedBloodCells")]
        [Display(Name = "Эритроциты свежие")]
        public double FreshRedBloodCells { get; set; }

        [Column("AlteredRedBloodCells")]
        [Display(Name = "Эритроциты измененные")]
        public double AlteredRedBloodCells { get; set; }

        [Column("Protein")]
        [Display(Name = "Белок")]
        public double Protein { get; set; }
    }
}
