using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    public class ParametersNorms
    {
        [Display(Name = "Нижнее значение нормы температуры")]
        public double LowNormalTemperature { get; set; }
        
        [Display(Name = "Верхнее значение нормы температуры")]
        public double UpNormalTemperature { get; set; }
        
        [Display(Name = "Нижний предел нормы температуры")]
        public double LowCriticalTemperature { get; set; }
        
        [Display(Name = "Верхний предел нормы температуры")]
        public double UpCriticalTemperature { get; set; }
        
        [Display(Name = "Нижнее значение нормы сатурации")]
        public double LowNormalSaturation { get; set; }
        
        [Display(Name = "Нижний предел нормы сатурации")]
        public double LowCriticalSaturation { get; set; }
      
        [Display(Name = "Критический объем поражения легочной ткани в процентах")]
        public double UpCriticalLungDamage { get; set; }
        
        [Display(Name = "Верхний предел нормы С-реактивного белка")]
        public double UpCriticalCReactiveProtein { get; set; }
        
        [Display(Name = "Верхнее значение нормы  С-реактивного белка")]
        public double UpNormCReactiveProtein { get; set; }
    }
}
