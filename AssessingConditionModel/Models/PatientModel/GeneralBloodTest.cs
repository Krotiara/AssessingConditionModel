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
        public double Er { get; set; }

        [Column("Hb")]
        public double Hb { get; set; }

        [Column("Ley")]
        public double Ley { get; set; }

        [Column("GranPercent")]
        public double GranPercent { get; set; }

        [Column("Gran")]
        public double Gran { get; set; }

        [Column("LymPercent")]
        public double LymPercent { get; set; }

        [Column("MonoPercent")]
        public double MonoPercent { get; set; }

        [Column("Mono")]
        public double Mono { get; set; }

        [Column("Tr")]
        public double Tr { get; set; }

    }
}
