using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    public class ClinicalParameters
    {
        public int PatientId { get; set; }

        [Display(Name = "Температура")]
        public double Temperature { get; set; }

        [Display(Name = "Сатурация")]
        public double Saturation { get; set; }

        [Display(Name = "Кашель")]
        public bool IsCough { get; set; }

        [Display(Name = "Объем поражения легочной ткани")]
        public double LungTissueDamage { get; set; }

        [Display(Name = "Частота дыхательных движений")]
        public double FRM { get; set; }

        [Display(Name = "Частота сердечных сокращений")]
        public double HeartRate { get; set; }

        [Display(Name = "С-реактивный белок")]
        public double CReactiveProtein { get; set; }
    }
}
