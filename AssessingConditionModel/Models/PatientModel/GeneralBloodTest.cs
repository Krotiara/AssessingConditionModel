using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Er")]
        public double Er { get; set; }

        [Column("Hb")]
        [DisplayName("Hb")]
        public double Hb { get; set; }

        [Column("Ley")]
        [DisplayName("Ley")]
        public double Ley { get; set; }

        [Column("GranPercent")]
        [DisplayName("GranPercent")]
        public double GranPercent { get; set; }

        [Column("Gran")]
        [DisplayName("Gran")]
        public double Gran { get; set; }

        [Column("LymPercent")]
        [DisplayName("LymPercent")]
        public double LymPercent { get; set; }

        [Column("Lym")]
        [DisplayName("Lym")]
        public double Lym { get; set; }


        [Column("MonoPercent")]
        [DisplayName("MonoPercent")]
        public double MonoPercent { get; set; }

        [Column("Mono")]
        [DisplayName("Mono")]
        public double Mono { get; set; }

        [Column("Tr")]
        [DisplayName("Tr")]
        public double Tr { get; set; }

    }
}
