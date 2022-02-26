using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Лейкоциты")]
        public double WhiteBloodCells { get; set; }

        [Column("FreshRedBloodCells")]
        [DisplayName("Эритроциты свежие")]
        public double FreshRedBloodCells { get; set; }

        [Column("AlteredRedBloodCells")]
        [DisplayName("Эритроциты измененные")]
        public double AlteredRedBloodCells { get; set; }

        [Column("Protein")]
        [DisplayName("Белок")]
        public double Protein { get; set; }
    }
}
