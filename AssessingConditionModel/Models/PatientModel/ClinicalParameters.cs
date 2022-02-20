using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.PatientModel
{
    [Table("ClinicalParameters")]
    public class ClinicalParameters
    {

        public ClinicalParameters()
        {
            LungTissueDamage = new LungTissueDamage();
            //suitable constructor for entity type for awoid EF error No suitable constructor found for entity type
        }

        [Key]
        [Required]
        public int PatientId { get; set; }

        [Column("Date")]
        [Display(Name = "Дата снятия показателей")]
        public DateTime Date { get; set; }

        [Column("Temperature")]
        [Display(Name = "Температура")]
        public double Temperature { get; set; }

        [Column("Saturation")]
        [Display(Name = "Сатурация")]
        public double Saturation { get; set; }

        [Column("IsCough")]
        [Display(Name = "Кашель")]
        public bool IsCough { get; set; }

        //[Column("LungTissueDamage")]
        //[Display(Name = "Объем поражения легочной ткани")]
        //public double LungTissueDamage { get; set; }

        [Column("FRM")]
        [Display(Name = "Частота дыхательных движений")]
        public double FRM { get; set; }

        [Column("HeartRate")]
        [Display(Name = "Частота сердечных сокращений")]
        public double HeartRate { get; set; }

        [Column("CReactiveProtein")]
        [Display(Name = "С-реактивный белок")]
        public double CReactiveProtein { get; set; }


        public LungTissueDamage LungTissueDamage { get; set; }

        public GeneralBloodTest GeneralBloodTest { get; set; }

    }
}
