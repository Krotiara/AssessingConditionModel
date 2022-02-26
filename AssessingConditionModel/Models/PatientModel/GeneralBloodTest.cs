using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.PatientModel
{
    [Table("GeneralBloodTests")]
    public class GeneralBloodTest
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("Er")]
        [Display(Name = "Er")]
        public double Er { get; set; }

        [Column("Hb")]
        [Display(Name = "Hb")]
        public double Hb { get; set; }

        [Column("Ley")]
        [Display(Name = "Ley")]
        public double Ley { get; set; }

        [Column("GranPercent")]
        [Display(Name = "GranPercent")]
        public double GranPercent { get; set; }

        [Column("Gran")]
        [Display(Name = "Gran")]
        public double Gran { get; set; }

        [Column("LymPercent")]
        [Display(Name = "LymPercent")]
        public double LymPercent { get; set; }

        [Column("Lym")]
        [Display(Name = "Lym")]
        public double Lym { get; set; }


        [Column("MonoPercent")]
        [Display(Name = "MonoPercent")]
        public double MonoPercent { get; set; }

        [Column("Mono")]
        [Display(Name = "Mono")]
        public double Mono { get; set; }

        [Column("Tr")]
        [Display(Name = "Tr")]
        public double Tr { get; set; }

    }
}
